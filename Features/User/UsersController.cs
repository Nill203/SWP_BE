using Microsoft.AspNetCore.Mvc;
using BloodDonationBE.Features.Users.DTOs;
using Microsoft.AspNetCore.Authorization;
using BloodDonationBE.Common.Enums;
using System.Security.Claims;

namespace BloodDonationBE.Features.Users;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [AllowAnonymous] 
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


    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
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
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
        {
            return Unauthorized("Token không hợp lệ hoặc không chứa User ID.");
        }

        var updatedUser = await _userService.UpdateUserAsync(userId, dto);
        return Ok(updatedUser);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")]
    public async Task<IActionResult> UpdateUserByAdmin(int id, [FromBody] UserUpdateDto dto)
    {
        try
        {
            var updatedUser = await _userService.UpdateUserAsync(id, dto);
            return Ok(updatedUser);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")] 
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
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            await _userService.DeleteUserAsync(id);
            return NoContent(); 
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("staff/create")]
    [Authorize(Roles = nameof(UserRole.Admin))]
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

        [HttpGet("{id}/availability")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Staff)}")]
    public async Task<IActionResult> GetUserAvailability(int id)
    {
        try
        {
            var availabilityStatus = await _userService.GetUserAvailabilityStatusAsync(id);
            return Ok(availabilityStatus);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
