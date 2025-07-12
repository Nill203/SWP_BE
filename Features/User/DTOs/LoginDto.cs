using System.ComponentModel.DataAnnotations;

namespace BloodDonationBE.Features.Users.DTOs;

public class LoginDto
{
    /// <summary>
    /// Email của người dùng.
    /// </summary>
    /// <example>testuser@example.com</example>
    [Required(ErrorMessage = "Email không được để trống.")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
    public string Email { get; set; }

    /// <summary>
    /// Mật khẩu của người dùng.
    /// </summary>
    /// <example>password123</example>
    [Required(ErrorMessage = "Mật khẩu không được để trống.")]
    [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
    public string Password { get; set; }
}