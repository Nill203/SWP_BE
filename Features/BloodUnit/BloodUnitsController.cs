using Microsoft.AspNetCore.Mvc;
using BloodDonationBE.Features.BloodUnits.DTOs;
using Microsoft.AspNetCore.Authorization;
using BloodDonationBE.Common.Enums;
using System.Security.Claims;

namespace BloodDonationBE.Features.BloodUnits;

[ApiController]
[Route("api/blood-units")]
public class BloodUnitsController : ControllerBase
{
    private readonly IBloodUnitService _bloodUnitService;

    public BloodUnitsController(IBloodUnitService bloodUnitService)
    {
        _bloodUnitService = bloodUnitService;
    }

    // GET: api/blood-units
    [HttpGet]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")]
    public async Task<IActionResult> GetAllBloodUnits()
    {
        var units = await _bloodUnitService.GetAllAsync();
        return Ok(units);
    }

    // GET: api/blood-units/5
    [HttpGet("{id}")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")]
    public async Task<IActionResult> GetBloodUnitById(int id)
    {
        try
        {
            var unit = await _bloodUnitService.GetByIdAsync(id);
            return Ok(unit);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // POST: api/blood-units/manual-create
    [HttpPost("manual-create")]
    [Authorize(Roles = nameof(UserRole.Admin))] // Chỉ Admin được tạo thủ công
    public async Task<IActionResult> CreateManualBloodUnit([FromBody] ManualCreateBloodUnitDto dto)
    {
        var createdUnit = await _bloodUnitService.CreateManualAsync(dto);
        return CreatedAtAction(nameof(GetBloodUnitById), new { id = createdUnit.Id }, createdUnit);
    }

    // PATCH: api/blood-units/5/status
    [HttpPatch("{id}/status")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")]
    public async Task<IActionResult> UpdateBloodUnitStatus(int id, [FromBody] UpdateBloodUnitDto dto)
    {
        try
        {
            // Lấy ID của nhân viên đang thực hiện hành động từ token
            var verifierIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(verifierIdString) || !int.TryParse(verifierIdString, out var verifierId))
            {
                return Unauthorized("Token không hợp lệ.");
            }

            var updatedUnit = await _bloodUnitService.UpdateStatusAsync(id, dto, verifierId);
            return Ok(updatedUnit);
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
}
