using Microsoft.AspNetCore.Mvc;
using BloodDonationBE.Features.Users.DTOs;
using Microsoft.AspNetCore.Authorization;
using BloodDonationBE.Common.Enums;
using System.Security.Claims;

namespace BloodDonationBE.Features.Users;

[ApiController]
[Route("api/users")] // Tiền tố chung cho tất cả các API trong controller này
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // --- Endpoints công khai (Public) ---

    [HttpPost("register")]
    [AllowAnonymous] // Bất kỳ ai cũng có thể gọi API này
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            var message = await _userService.RegisterAsync(dto);
            return Ok(new { message });
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var result = await _userService.LoginAsync(dto);
            return Ok(result);
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("confirm-registration")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmRegistration([FromQuery] string token)
    {
        try
        {
            var message = await _userService.ConfirmRegistrationAsync(token);
            return Ok(new { message });
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // --- Endpoints yêu cầu xác thực ---

    [HttpGet("me")]
    [Authorize] // Yêu cầu người dùng phải đăng nhập (có JWT token hợp lệ)
    public async Task<IActionResult> GetCurrentUser()
    {
        // Lấy UserId từ token và kiểm tra null
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
        {
            return Unauthorized("Token không hợp lệ hoặc không chứa User ID.");
        }

        var user = await _userService.GetUserByIdAsync(userId);
        return Ok(user);
    }
    
    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateCurrentUser([FromBody] UserUpdateDto dto)
    {
        // Lấy UserId từ token và kiểm tra null
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
        {
            return Unauthorized("Token không hợp lệ hoặc không chứa User ID.");
        }

        var updatedUser = await _userService.UpdateUserAsync(userId, dto);
        return Ok(updatedUser);
    }


    // --- Endpoints yêu cầu quyền Admin/Staff ---

    [HttpGet]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")] // Chỉ Admin hoặc Staff
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
    
    [HttpPost("admin/create")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")]
    public async Task<IActionResult> CreateByAdmin([FromBody] AdminCreateUserDto dto)
    {
        var user = await _userService.CreateByAdminAsync(dto);
        return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = nameof(UserRole.Admin))] // Chỉ Admin
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            await _userService.DeleteUserAsync(id);
            return NoContent(); // Trả về 204 No Content khi xóa thành công
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("staff/create")]
    [Authorize(Roles = nameof(UserRole.Admin))] // Chỉ Admin
    public async Task<IActionResult> CreateStaff([FromBody] CreateStaffDto dto)
    {
        try
        {
            var user = await _userService.CreateStaffAsync(dto);
            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
