using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectFinally.Models.DTOs.AdSense;
using ProjectFinally.Services.Interfaces;

namespace ProjectFinally.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdSenseCampaignsController : ControllerBase
{
    private readonly IAdSenseCampaignService _campaignService;
    private readonly ILogger<AdSenseCampaignsController> _logger;

    public AdSenseCampaignsController(IAdSenseCampaignService campaignService, ILogger<AdSenseCampaignsController> logger)
    {
        _campaignService = campaignService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdSenseCampaignDto>>> GetAllCampaigns([FromQuery] int? userId = null)
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

            // Admin: Ve TODAS las campañas o filtra por userId si se proporciona
            if (roleClaim == "Admin")
            {
                if (userId.HasValue)
                {
                    var filteredCampaigns = await _campaignService.GetCampaignsByOwnerIdAsync(userId.Value);
                    return Ok(filteredCampaigns);
                }
                var allCampaigns = await _campaignService.GetAllCampaignsAsync();
                return Ok(allCampaigns);
            }
            // Partner: Ve TODAS las campañas
            else if (roleClaim == "Partner")
            {
                var allCampaigns = await _campaignService.GetAllCampaignsAsync();
                return Ok(allCampaigns);
            }
            // Employee: Ve TODAS las campañas (gestión de operaciones)
            else if (roleClaim == "Employee")
            {
                var allCampaigns = await _campaignService.GetAllCampaignsAsync();
                return Ok(allCampaigns);
            }
            // Viewer: Ve TODAS las campañas (solo lectura)
            else if (roleClaim == "Viewer")
            {
                var allCampaigns = await _campaignService.GetAllCampaignsAsync();
                return Ok(allCampaigns);
            }
            // ContentManager: NO puede ver campañas
            else
            {
                return Forbid();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all campaigns");
            return StatusCode(500, new { message = "An error occurred while retrieving campaigns" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AdSenseCampaignDto>> GetCampaign(int id)
    {
        try
        {
            var campaign = await _campaignService.GetCampaignByIdAsync(id);
            if (campaign == null)
                return NotFound(new { message = $"Campaign with ID {id} not found" });

            return Ok(campaign);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving campaign {CampaignId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the campaign" });
        }
    }

    [HttpGet("channel/{channelId}")]
    public async Task<ActionResult<IEnumerable<AdSenseCampaignDto>>> GetCampaignsByChannel(int channelId)
    {
        try
        {
            var campaigns = await _campaignService.GetCampaignsByChannelAsync(channelId);
            return Ok(campaigns);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving campaigns for channel {ChannelId}", channelId);
            return StatusCode(500, new { message = "An error occurred while retrieving campaigns" });
        }
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<AdSenseCampaignDto>>> GetActiveCampaigns()
    {
        try
        {
            var campaigns = await _campaignService.GetActiveCampaignsAsync();
            return Ok(campaigns);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active campaigns");
            return StatusCode(500, new { message = "An error occurred while retrieving active campaigns" });
        }
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<AdSenseCampaignDto>>> GetCampaignsByStatus(string status)
    {
        try
        {
            var campaigns = await _campaignService.GetCampaignsByStatusAsync(status);
            return Ok(campaigns);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving campaigns with status {Status}", status);
            return StatusCode(500, new { message = "An error occurred while retrieving campaigns" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Partner,Employee")]
    public async Task<ActionResult<AdSenseCampaignDto>> CreateCampaign([FromBody] CreateAdSenseCampaignDto createDto)
    {
        try
        {
            var campaign = await _campaignService.CreateCampaignAsync(createDto);
            return CreatedAtAction(nameof(GetCampaign), new { id = campaign.CampaignId }, campaign);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating campaign");
            return StatusCode(500, new { message = "An error occurred while creating the campaign" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Partner,Employee")]
    public async Task<ActionResult<AdSenseCampaignDto>> UpdateCampaign(int id, [FromBody] UpdateAdSenseCampaignDto updateDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            // Obtener la campaña existente para verificar propiedad
            var existingCampaign = await _campaignService.GetCampaignByIdAsync(id);
            if (existingCampaign == null)
                return NotFound(new { message = $"Campaign with ID {id} not found" });

            // Admin, Partner y Employee: pueden editar cualquier campaña
            // (Employee gestiona operaciones de todas las campañas)

            var campaign = await _campaignService.UpdateCampaignAsync(id, updateDto);
            return Ok(campaign);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating campaign {CampaignId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the campaign" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Partner,Employee")]
    public async Task<IActionResult> DeleteCampaign(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            // Obtener la campaña existente para verificar propiedad
            var existingCampaign = await _campaignService.GetCampaignByIdAsync(id);
            if (existingCampaign == null)
                return NotFound(new { message = $"Campaign with ID {id} not found" });

            // Admin, Partner y Employee: pueden eliminar cualquier campaña
            // (Employee gestiona operaciones de todas las campañas)

            var result = await _campaignService.DeleteCampaignAsync(id);
            if (!result)
                return NotFound(new { message = $"Campaign with ID {id} not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting campaign {CampaignId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the campaign" });
        }
    }
}
