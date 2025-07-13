using Microsoft.AspNetCore.Mvc;
using BloodDonationBE.Features.CampaignRegistrations.DTOs;
using Microsoft.AspNetCore.Authorization;
using BloodDonationBE.Common.Enums;
using System.Security.Claims;

namespace BloodDonationBE.Features.CampaignRegistrations;

[ApiController]
[Route("api/campaign-registrations")]
public class CampaignRegistrationsController : ControllerBase
{
    private readonly ICampaignRegistrationService _registrationService;

    public CampaignRegistrationsController(ICampaignRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    // POST: api/campaign-registrations
    /// <summary>
    /// Người dùng đã đăng nhập tạo một lượt đăng ký mới cho một sự kiện.
    /// </summary>
    [HttpPost]
    [Authorize] // Yêu cầu phải đăng nhập
    public async Task<IActionResult> CreateRegistration([FromBody] CreateRegistrationDto dto)
    {
        try
        {
            // Lấy UserId từ token của người dùng đang đăng nhập
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Token không hợp lệ.");
            }

            var result = await _registrationService.CreateRegistrationAsync(userId, dto);
            return CreatedAtAction(nameof(GetAllRegistrations), new { id = result.Id }, result);
        }
        catch (BadHttpRequestException ex)
        {
            return Conflict(new { message = ex.Message }); // Trả về 409 Conflict nếu đã đăng ký
        }
    }

    // GET: api/campaign-registrations?campaignId=...
    /// <summary>
    /// Lấy danh sách tất cả các lượt đăng ký. Có thể lọc theo ID của sự kiện.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")] // Chỉ admin/staff mới được xem tất cả
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
    /// <summary>
    /// Cập nhật trạng thái của một lượt đăng ký (dành cho admin/staff).
    /// </summary>
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
    /// <summary>
    /// Lấy lịch sử hiến máu của chính người dùng đang đăng nhập.
    /// </summary>
    [HttpGet("history/user/me")]
    [Authorize] // Bất kỳ ai đăng nhập cũng có thể xem lịch sử của mình
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
    /// <summary>
    /// Lấy lịch sử hiến máu của một người dùng cụ thể (dành cho admin/staff).
    /// </summary>
    [HttpGet("history/user/{userId}")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")]
    public async Task<IActionResult> GetDonationHistoryByUserId(int userId)
    {
        var history = await _registrationService.GetDonationHistoryByUserIdAsync(userId);
        return Ok(history);
    }

       /// <summary>
    /// Lấy danh sách các sự kiện mà người dùng hiện tại đã đăng ký.
    /// </summary>
    [HttpGet("me")]
    [Authorize] // Bất kỳ ai đăng nhập cũng có thể xem đăng ký của mình
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

    /// <summary>
    /// Người dùng tự hủy một lượt đăng ký.
    /// </summary>
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
        catch (UnauthorizedAccessException ex) { return Forbid(); } // Trả về 403 Forbidden
        catch (BadHttpRequestException ex) { return BadRequest(new { message = ex.Message }); }
    }
}
