using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectFinally.Models.DTOs.YouTube;
using ProjectFinally.Services.Interfaces;

namespace ProjectFinally.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class YouTubeChannelsController : ControllerBase
{
    private readonly IYouTubeChannelService _channelService;
    private readonly ILogger<YouTubeChannelsController> _logger;

    public YouTubeChannelsController(IYouTubeChannelService channelService, ILogger<YouTubeChannelsController> logger)
    {
        _channelService = channelService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<YouTubeChannelDto>>> GetAllChannels([FromQuery] int? userId = null)
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

            // Admin: Ve TODOS los canales o filtra por userId si se proporciona
            if (roleClaim == "Admin")
            {
                if (userId.HasValue)
                {
                    var filteredChannels = await _channelService.GetChannelsByOwnerIdAsync(userId.Value);
                    return Ok(filteredChannels);
                }
                var allChannels = await _channelService.GetAllChannelsAsync();
                return Ok(allChannels);
            }
            // Partner: Ve TODOS los canales
            else if (roleClaim == "Partner")
            {
                var allChannels = await _channelService.GetAllChannelsAsync();
                return Ok(allChannels);
            }
            // ContentManager: Solo ve SUS canales
            else if (roleClaim == "ContentManager")
            {
                var ownChannels = await _channelService.GetChannelsByOwnerIdAsync(currentUserId);
                return Ok(ownChannels);
            }
            // Viewer: Ve TODOS los canales (solo lectura)
            else if (roleClaim == "Viewer")
            {
                var allChannels = await _channelService.GetAllChannelsAsync();
                return Ok(allChannels);
            }
            // Employee: NO puede ver canales
            else
            {
                return Forbid();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving channels");
            return StatusCode(500, new { message = "An error occurred while retrieving channels" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<YouTubeChannelDto>> GetChannel(int id)
    {
        try
        {
            var channel = await _channelService.GetChannelByIdAsync(id);
            if (channel == null)
                return NotFound(new { message = $"Channel with ID {id} not found" });

            return Ok(channel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving channel {ChannelId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the channel" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Partner,ContentManager")]
    public async Task<ActionResult<YouTubeChannelDto>> CreateChannel([FromBody] CreateYouTubeChannelDto createChannelDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            // Para ContentManager, asignar automáticamente como owner
            // Para Admin y Partner, puede ser null (gestión general)
            int? ownerId = null;
            if (roleClaim == "ContentManager")
            {
                ownerId = userId;
            }

            var channel = await _channelService.CreateChannelAsync(createChannelDto, ownerId);
            return CreatedAtAction(nameof(GetChannel), new { id = channel.ChannelId }, channel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating channel");
            return StatusCode(500, new { message = "An error occurred while creating the channel" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Partner,ContentManager")]
    public async Task<ActionResult<YouTubeChannelDto>> UpdateChannel(int id, [FromBody] UpdateYouTubeChannelDto updateChannelDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            // Admin y Partner: pueden editar cualquier canal
            // ContentManager: solo puede editar sus propios canales
            if (roleClaim == "ContentManager")
            {
                var existingChannel = await _channelService.GetChannelByIdAsync(id);
                if (existingChannel == null)
                    return NotFound(new { message = $"Channel with ID {id} not found" });

                // Verificar que el canal pertenezca al usuario
                if (existingChannel.OwnerId != userId)
                {
                    return Forbid();
                }
            }

            var channel = await _channelService.UpdateChannelAsync(id, updateChannelDto);
            if (channel == null)
                return NotFound(new { message = $"Channel with ID {id} not found" });

            return Ok(channel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating channel {ChannelId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the channel" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Partner,ContentManager")]
    public async Task<IActionResult> DeleteChannel(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            // Admin y Partner: pueden eliminar cualquier canal
            // ContentManager: solo puede eliminar sus propios canales
            if (roleClaim == "ContentManager")
            {
                var existingChannel = await _channelService.GetChannelByIdAsync(id);
                if (existingChannel == null)
                    return NotFound(new { message = $"Channel with ID {id} not found" });

                // Verificar que el canal pertenezca al usuario
                if (existingChannel.OwnerId != userId)
                {
                    return Forbid();
                }
            }

            var result = await _channelService.DeleteChannelAsync(id);
            if (!result)
                return NotFound(new { message = $"Channel with ID {id} not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting channel {ChannelId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the channel" });
        }
    }
}
