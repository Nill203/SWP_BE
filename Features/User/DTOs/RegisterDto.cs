using System.ComponentModel.DataAnnotations;

namespace BloodDonationBE.Features.Users.DTOs;

public class RegisterDto
{
    /// <summary>
    /// Họ và tên đầy đủ của người dùng.
    /// </summary>
    /// <example>Nguyễn Văn A</example>
    [Required(ErrorMessage = "Họ và tên không được để trống.")]
    [StringLength(100)]
    public string FullName { get; set; }

    /// <summary>
    /// Địa chỉ email, sẽ được dùng để đăng nhập.
    /// </summary>
    /// <example>nguyenvana@example.com</example>
    [Required(ErrorMessage = "Email không được để trống.")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
    public string Email { get; set; }
    
    /// <summary>
    /// Số điện thoại của người dùng.
    /// </summary>
    /// <example>0912345678</example>
    [Required(ErrorMessage = "Số điện thoại không được để trống.")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có đúng 10 ký tự.")]
    [Phone]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Mật khẩu (tối thiểu 6 ký tự).
    /// </summary>
    /// <example>password123</example>
    [Required(ErrorMessage = "Mật khẩu không được để trống.")]
    [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
    public string Password { get; set; }

    /// <summary>
    /// Xác nhận mật khẩu.
    /// </summary>
    /// <example>password123</example>
    [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu.")]
    [Compare(nameof(Password), ErrorMessage = "Mật khẩu xác nhận không khớp.")]
    public string ConfirmPassword { get; set; }
}