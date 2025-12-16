using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectFinally.Models.DTOs.YouTube;
using ProjectFinally.Services.Interfaces;

namespace ProjectFinally.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VideosController : ControllerBase
{
    private readonly IVideoService _videoService;
    private readonly IYouTubeChannelService _channelService;
    private readonly ILogger<VideosController> _logger;

    public VideosController(IVideoService videoService, IYouTubeChannelService channelService, ILogger<VideosController> logger)
    {
        _videoService = videoService;
        _channelService = channelService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VideoDto>>> GetAllVideos([FromQuery] int? userId = null)
    {
        try
        {
            // Obtener el userId y rol del token JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int currentUserId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            // Admin: Ve TODOS los videos o filtra por userId si se proporciona
            if (roleClaim == "Admin")
            {
                if (userId.HasValue)
                {
                    var filteredVideos = await _videoService.GetVideosByOwnerIdAsync(userId.Value);
                    return Ok(filteredVideos);
                }
                var allVideos = await _videoService.GetAllVideosAsync();
                return Ok(allVideos);
            }
            // Partner: Ve TODOS los videos
            else if (roleClaim == "Partner")
            {
                var allVideos = await _videoService.GetAllVideosAsync();
                return Ok(allVideos);
            }
            // ContentManager: Solo ve SUS videos (de sus canales)
            else if (roleClaim == "ContentManager")
            {
                var ownVideos = await _videoService.GetVideosByOwnerIdAsync(currentUserId);
                return Ok(ownVideos);
            }
            // Viewer: Ve TODOS los videos (solo lectura)
            else if (roleClaim == "Viewer")
            {
                var allVideos = await _videoService.GetAllVideosAsync();
                return Ok(allVideos);
            }
            // Employee: NO puede ver videos
            else
            {
                return Forbid();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all videos");
            return StatusCode(500, new { message = "An error occurred while retrieving videos" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VideoDto>> GetVideo(int id)
    {
        try
        {
            var video = await _videoService.GetVideoByIdAsync(id);
            if (video == null)
                return NotFound(new { message = $"Video with ID {id} not found" });

            return Ok(video);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving video {VideoId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the video" });
        }
    }

    [HttpGet("channel/{channelId}")]
    public async Task<ActionResult<IEnumerable<VideoDto>>> GetVideosByChannel(int channelId)
    {
        try
        {
            var videos = await _videoService.GetVideosByChannelAsync(channelId);
            return Ok(videos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving videos for channel {ChannelId}", channelId);
            return StatusCode(500, new { message = "An error occurred while retrieving videos" });
        }
    }

    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<VideoDto>>> GetVideosByCategory(int categoryId)
    {
        try
        {
            var videos = await _videoService.GetVideosByCategoryAsync(categoryId);
            return Ok(videos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving videos for category {CategoryId}", categoryId);
            return StatusCode(500, new { message = "An error occurred while retrieving videos" });
        }
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<VideoDto>>> GetVideosByStatus(string status)
    {
        try
        {
            var videos = await _videoService.GetVideosByStatusAsync(status);
            return Ok(videos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving videos with status {Status}", status);
            return StatusCode(500, new { message = "An error occurred while retrieving videos" });
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<VideoDto>>> SearchVideos([FromQuery] string term)
    {
        try
        {
            var videos = await _videoService.SearchVideosAsync(term);
            return Ok(videos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching videos with term {SearchTerm}", term);
            return StatusCode(500, new { message = "An error occurred while searching videos" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Partner,ContentManager")]
    public async Task<ActionResult<VideoDto>> CreateVideo([FromBody] CreateVideoDto createVideoDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            // Validar que el canal existe
            var channel = await _channelService.GetChannelByIdAsync(createVideoDto.ChannelId);
            if (channel == null)
            {
                return BadRequest(new { message = "El canal especificado no existe" });
            }

            // Para ContentManager, verificar que el canal les pertenezca
            // Admin y Partner pueden crear videos en cualquier canal
            if (roleClaim == "ContentManager")
            {
                if (channel.OwnerId != userId)
                {
                    return Forbid();
                }
            }

            var video = await _videoService.CreateVideoAsync(createVideoDto);
            return CreatedAtAction(nameof(GetVideo), new { id = video.VideoId }, video);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating video");
            return StatusCode(500, new { message = "An error occurred while creating the video" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Partner,ContentManager")]
    public async Task<ActionResult<VideoDto>> UpdateVideo(int id, [FromBody] UpdateVideoDto updateVideoDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            // Para ContentManager, verificar que el video pertenezca a un canal del usuario
            // Admin y Partner pueden editar cualquier video
            if (roleClaim == "ContentManager")
            {
                var existingVideo = await _videoService.GetVideoByIdAsync(id);
                if (existingVideo == null)
                    return NotFound(new { message = $"Video with ID {id} not found" });

                // Verificar que el canal del video pertenezca al usuario
                if (existingVideo.ChannelOwnerId != userId)
                {
                    return Forbid();
                }
            }

            var video = await _videoService.UpdateVideoAsync(id, updateVideoDto);
            if (video == null)
                return NotFound(new { message = $"Video with ID {id} not found" });

            return Ok(video);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating video {VideoId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the video" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Partner,ContentManager")]
    public async Task<IActionResult> DeleteVideo(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            // Para ContentManager, verificar que el video pertenezca a un canal del usuario
            // Admin y Partner pueden eliminar cualquier video
            if (roleClaim == "ContentManager")
            {
                var existingVideo = await _videoService.GetVideoByIdAsync(id);
                if (existingVideo == null)
                    return NotFound(new { message = $"Video with ID {id} not found" });

                // Verificar que el canal del video pertenezca al usuario
                if (existingVideo.ChannelOwnerId != userId)
                {
                    return Forbid();
                }
            }

            var result = await _videoService.DeleteVideoAsync(id);
            if (!result)
                return NotFound(new { message = $"Video with ID {id} not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting video {VideoId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the video" });
        }
    }
}
