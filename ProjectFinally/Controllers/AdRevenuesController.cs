using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectFinally.Models.DTOs.AdSense;
using ProjectFinally.Services.Interfaces;

namespace ProjectFinally.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdRevenuesController : ControllerBase
{
    private readonly IAdRevenueService _revenueService;
    private readonly ILogger<AdRevenuesController> _logger;

    public AdRevenuesController(IAdRevenueService revenueService, ILogger<AdRevenuesController> logger)
    {
        _revenueService = revenueService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdRevenueDto>>> GetAllRevenues()
    {
        try
        {
            var revenues = await _revenueService.GetAllRevenuesAsync();
            return Ok(revenues);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all revenues");
            return StatusCode(500, new { message = "An error occurred while retrieving revenues" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AdRevenueDto>> GetRevenue(int id)
    {
        try
        {
            var revenue = await _revenueService.GetRevenueByIdAsync(id);
            if (revenue == null)
                return NotFound(new { message = $"Revenue with ID {id} not found" });

            return Ok(revenue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving revenue {RevenueId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the revenue" });
        }
    }

    [HttpGet("video/{videoId}")]
    public async Task<ActionResult<IEnumerable<AdRevenueDto>>> GetRevenuesByVideo(int videoId)
    {
        try
        {
            var revenues = await _revenueService.GetRevenuesByVideoAsync(videoId);
            return Ok(revenues);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving revenues for video {VideoId}", videoId);
            return StatusCode(500, new { message = "An error occurred while retrieving revenues" });
        }
    }

    [HttpGet("campaign/{campaignId}")]
    public async Task<ActionResult<IEnumerable<AdRevenueDto>>> GetRevenuesByCampaign(int campaignId)
    {
        try
        {
            var revenues = await _revenueService.GetRevenuesByCampaignAsync(campaignId);
            return Ok(revenues);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving revenues for campaign {CampaignId}", campaignId);
            return StatusCode(500, new { message = "An error occurred while retrieving revenues" });
        }
    }

    [HttpGet("daterange")]
    public async Task<ActionResult<IEnumerable<AdRevenueDto>>> GetRevenuesByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            var revenues = await _revenueService.GetRevenuesByDateRangeAsync(startDate, endDate);
            return Ok(revenues);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving revenues for date range {StartDate} to {EndDate}", startDate, endDate);
            return StatusCode(500, new { message = "An error occurred while retrieving revenues" });
        }
    }

    [HttpGet("total")]
    [Authorize(Roles = "Admin,ContentManager,Analyst")]
    public async Task<ActionResult<decimal>> GetTotalRevenue()
    {
        try
        {
            var total = await _revenueService.GetTotalRevenueAsync();
            return Ok(new { totalRevenue = total });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating total revenue");
            return StatusCode(500, new { message = "An error occurred while calculating total revenue" });
        }
    }

    [HttpGet("total/campaign/{campaignId}")]
    [Authorize(Roles = "Admin,ContentManager,Analyst")]
    public async Task<ActionResult<decimal>> GetTotalRevenueByCampaign(int campaignId)
    {
        try
        {
            var total = await _revenueService.GetTotalRevenueByCampaignAsync(campaignId);
            return Ok(new { totalRevenue = total, campaignId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating total revenue for campaign {CampaignId}", campaignId);
            return StatusCode(500, new { message = "An error occurred while calculating campaign revenue" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,ContentManager")]
    public async Task<ActionResult<AdRevenueDto>> CreateRevenue([FromBody] CreateAdRevenueDto createDto)
    {
        try
        {
            var revenue = await _revenueService.CreateRevenueAsync(createDto);
            return CreatedAtAction(nameof(GetRevenue), new { id = revenue.RevenueId }, revenue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating revenue");
            return StatusCode(500, new { message = "An error occurred while creating the revenue" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,ContentManager")]
    public async Task<ActionResult<AdRevenueDto>> UpdateRevenue(int id, [FromBody] UpdateAdRevenueDto updateDto)
    {
        try
        {
            var revenue = await _revenueService.UpdateRevenueAsync(id, updateDto);
            if (revenue == null)
                return NotFound(new { message = $"Revenue with ID {id} not found" });

            return Ok(revenue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating revenue {RevenueId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the revenue" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRevenue(int id)
    {
        try
        {
            var result = await _revenueService.DeleteRevenueAsync(id);
            if (!result)
                return NotFound(new { message = $"Revenue with ID {id} not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting revenue {RevenueId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the revenue" });
        }
    }
}
