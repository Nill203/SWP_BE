using Microsoft.AspNetCore.Mvc;
using BloodDonationBE.Features.BloodRequests.DTOs;
using Microsoft.AspNetCore.Authorization;
using BloodDonationBE.Common.Enums;
using System.Security.Claims;

namespace BloodDonationBE.Features.BloodRequests;

[ApiController]
[Route("api/blood-requests")]
public class BloodRequestsController : ControllerBase
{
    private readonly IBloodRequestService _requestService;

    public BloodRequestsController(IBloodRequestService requestService)
    {
        _requestService = requestService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateRequest([FromBody] CreateRequestDto dto)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
        {
            return Unauthorized("Token không hợp lệ.");
        }
        var createdRequest = await _requestService.CreateRequestAsync(userId, dto);
        return CreatedAtAction(nameof(GetRequestById), new { id = createdRequest.Id }, createdRequest);
    }

    [HttpGet]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")]
    public async Task<IActionResult> GetAllRequests()
    {
        var requests = await _requestService.GetAllRequestsAsync();
        return Ok(requests);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")]
    public async Task<IActionResult> GetRequestById(int id)
    {
        try
        {
            var request = await _requestService.GetRequestByIdAsync(id);
            return Ok(request);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/verify")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")]
    public async Task<IActionResult> VerifyRequest(int id, [FromBody] UpdateRequestStatusDto dto)
    {
        try
        {
            var staffIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(staffIdString) || !int.TryParse(staffIdString, out var staffId))
            {
                return Unauthorized("Token không hợp lệ.");
            }
            var result = await _requestService.VerifyRequestAsync(id, staffId, dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (BadHttpRequestException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPatch("{id}/process")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> ProcessRequest(int id, [FromBody] UpdateRequestStatusDto dto)
    {
        try
        {
            var adminIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(adminIdString) || !int.TryParse(adminIdString, out var adminId))
            {
                return Unauthorized("Token không hợp lệ.");
            }
            var result = await _requestService.ProcessRequestAsync(id, adminId, dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (BadHttpRequestException ex) { return BadRequest(new { message = ex.Message }); }
    }

    // ==> API ENDPOINT MỚI
    [HttpPatch("{id}/fulfill")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")]
    public async Task<IActionResult> FulfillRequest(int id)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Token không hợp lệ.");
            }
            var result = await _requestService.FulfillRequestAsync(id, userId);
            return Ok(result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (BadHttpRequestException ex) { return BadRequest(new { message = ex.Message }); }
    }
}
