using Microsoft.AspNetCore.Mvc;
using BloodDonationBE.Features.Hospitals.DTOs;
using Microsoft.AspNetCore.Authorization;
using BloodDonationBE.Common.Enums;

namespace BloodDonationBE.Features.Hospitals;

[ApiController]
[Route("api/hospitals")]
public class HospitalsController : ControllerBase
{
    private readonly IHospitalService _hospitalService;

    public HospitalsController(IHospitalService hospitalService)
    {
        _hospitalService = hospitalService;
    }

    // POST: api/hospitals
    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))] // Chỉ Admin được tạo bệnh viện mới
    public async Task<IActionResult> CreateHospital([FromBody] CreateHospitalDto dto)
    {
        try
        {
            var createdHospital = await _hospitalService.CreateHospitalAsync(dto);
            return CreatedAtAction(nameof(GetHospitalById), new { id = createdHospital.Id }, createdHospital);
        }
        catch (BadHttpRequestException ex)
        {
            return Conflict(new { message = ex.Message }); // Trả về 409 Conflict nếu tên đã tồn tại
        }
    }

    // GET: api/hospitals
    [HttpGet]
    [AllowAnonymous] // Cho phép tất cả mọi người xem danh sách bệnh viện
    public async Task<IActionResult> GetAllHospitals()
    {
        var hospitals = await _hospitalService.GetAllHospitalsAsync();
        return Ok(hospitals);
    }

    // GET: api/hospitals/5
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetHospitalById(int id)
    {
        try
        {
            var hospital = await _hospitalService.GetHospitalByIdAsync(id);
            return Ok(hospital);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // PATCH: api/hospitals/5
    [HttpPatch("{id}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> UpdateHospital(int id, [FromBody] UpdateHospitalDto dto)
    {
        try
        {
            var updatedHospital = await _hospitalService.UpdateHospitalAsync(id, dto);
            return Ok(updatedHospital);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // DELETE: api/hospitals/5
    [HttpDelete("{id}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> DeleteHospital(int id)
    {
        try
        {
            await _hospitalService.DeleteHospitalAsync(id);
            return NoContent(); // Trả về 204 No Content khi xóa thành công
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
