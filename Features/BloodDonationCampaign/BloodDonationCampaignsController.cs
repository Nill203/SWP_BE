using Microsoft.AspNetCore.Mvc;
using BloodDonationBE.Features.BloodDonationCampaigns.DTOs;
using Microsoft.AspNetCore.Authorization;
using BloodDonationBE.Common.Enums;

namespace BloodDonationBE.Features.BloodDonationCampaigns;

[ApiController]
[Route("api/blood-donation-campaigns")]
public class BloodDonationCampaignsController : ControllerBase
{
    private readonly IBloodDonationCampaignService _campaignService;

    public BloodDonationCampaignsController(IBloodDonationCampaignService campaignService)
    {
        _campaignService = campaignService;
    }

    // POST: api/blood-donation-campaigns
    [HttpPost]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")]
    public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignDto dto)
    {
        var createdCampaign = await _campaignService.CreateCampaignAsync(dto);
        return CreatedAtAction(nameof(GetCampaignById), new { id = createdCampaign.Id }, createdCampaign);
    }

    // GET: api/blood-donation-campaigns
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCampaigns()
    {
        var campaigns = await _campaignService.GetAllCampaignsAsync();
        return Ok(campaigns);
    }

    // GET: api/blood-donation-campaigns/search?start=...&end=...
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchCampaigns([FromQuery] DateTime? start, [FromQuery] DateTime? end)
    {
        var campaigns = await _campaignService.SearchCampaignsByDateRangeAsync(start, end);
        return Ok(campaigns);
    }

    // GET: api/blood-donation-campaigns/5
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCampaignById(int id)
    {
        try
        {
            var campaign = await _campaignService.GetCampaignByIdAsync(id);
            return Ok(campaign);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // PATCH: api/blood-donation-campaigns/5
    [HttpPatch("{id}")] // Dùng PATCH cho việc cập nhật một phần
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")]
    public async Task<IActionResult> UpdateCampaign(int id, [FromBody] UpdateCampaignDto dto)
    {
        try
        {
            var updatedCampaign = await _campaignService.UpdateCampaignAsync(id, dto);
            return Ok(updatedCampaign);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // DELETE: api/blood-donation-campaigns/5
    [HttpDelete("{id}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> DeleteCampaign(int id)
    {
        try
        {
            await _campaignService.DeleteCampaignAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
