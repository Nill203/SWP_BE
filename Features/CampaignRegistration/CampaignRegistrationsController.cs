using Microsoft.AspNetCore.Mvc;
using BloodDonationBE.Features.CampaignRegistrations.DTOs;
using Microsoft.AspNetCore.Authorization;
using BloodDonationBE.Common.Enums;
using System.Security.Claims;
using BloodDonationBE.Features.Users;

namespace BloodDonationBE.Features.CampaignRegistrations;

[ApiController]
[Route("api/campaign-registrations")]
public class CampaignRegistrationsController : ControllerBase
{
    private readonly ICampaignRegistrationService _registrationService;
    private readonly IUserService _userService;
    public CampaignRegistrationsController(ICampaignRegistrationService registrationService, IUserService userService)
    {
        _registrationService = registrationService;
        _userService = userService;
    }

    // POST: api/campaign-registrations
    [HttpPost]
    [Authorize] 
    public async Task<IActionResult> CreateRegistration([FromBody] CreateRegistrationDto dto)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Token không hợp lệ.");
            }

            var availability = await _userService.GetUserAvailabilityStatusAsync(userId);
            if (!availability.IsAvailable)
            {
                return BadRequest(new { message = availability.StatusMessage });
            }
            var result = await _registrationService.CreateRegistrationAsync(userId, dto);
            return CreatedAtAction(nameof(GetAllRegistrations), new { id = result.Id }, result);
        }
        catch (BadHttpRequestException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // GET: api/campaign-registrations?campaignId=...
    [HttpGet]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")]
    public async Task<IActionResult> GetAllRegistrations([FromQuery] int? campaignId)
    {
        if (campaignId.HasValue)
        {
            var byCampaign = await _registrationService.GetRegistrationsByCampaignIdAsync(campaignId.Value);
            return Ok(byCampaign);
        }

        var all = await _registrationService.GetAllRegistrationsAsync();
        return Ok(all);
    }

    // PATCH: api/campaign-registrations/5
    [HttpPatch("{id}")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")]
    public async Task<IActionResult> UpdateRegistration(int id, [FromBody] UpdateRegistrationDto dto)
    {
        try
        {
            var result = await _registrationService.UpdateRegistrationAsync(id, dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET: api/campaign-registrations/history/user/me
    [HttpGet("history/user/me")]
    [Authorize]
    public async Task<IActionResult> GetMyDonationHistory()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
        {
            return Unauthorized("Token không hợp lệ.");
        }
        var history = await _registrationService.GetDonationHistoryByUserIdAsync(userId);
        return Ok(history);
    }

    // GET: api/campaign-registrations/history/user/5
    [HttpGet("history/user/{userId}")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")]
    public async Task<IActionResult> GetDonationHistoryByUserId(int userId)
    {
        var history = await _registrationService.GetDonationHistoryByUserIdAsync(userId);
        return Ok(history);
    }

    [HttpGet("me")]
    [Authorize] 
    public async Task<IActionResult> GetMyRegistrations()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
        {
            return Unauthorized("Token không hợp lệ.");
        }

        var registrations = await _registrationService.GetMyRegistrationsAsync(userId);
        return Ok(registrations);
    }

    [HttpPatch("{id}/cancel")]
    [Authorize]
    public async Task<IActionResult> CancelRegistration(int id)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Token không hợp lệ.");
            }

            var result = await _registrationService.CancelRegistrationAsync(id, userId);
            return Ok(result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (UnauthorizedAccessException ex) { return Forbid(); }
        catch (BadHttpRequestException ex) { return BadRequest(new { message = ex.Message }); }
    }
}
