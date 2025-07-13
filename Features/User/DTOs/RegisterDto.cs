using System.ComponentModel.DataAnnotations;
using BloodDonationBE.Common.Enums;

namespace BloodDonationBE.Features.Users.DTOs;

public class RegisterDto
{
    /// <summary>
    /// Họ và tên đầy đủ của người dùng.
    /// </summary>
    /// <example>Nguyễn Văn A</example>
    [Required(ErrorMessage = "Họ và tên không được để trống.")]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Địa chỉ email, sẽ được dùng để đăng nhập.
    /// </summary>
    /// <example>nguyenvana@example.com</example>
    [Required(ErrorMessage = "Email không được để trống.")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Số điện thoại của người dùng.
    /// </summary>
    /// <example>0912345678</example>
    [Required(ErrorMessage = "Số điện thoại không được để trống.")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có đúng 10 ký tự.")]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;
    
    // --- CÁC TRƯỜNG MỚI ĐƯỢC BỔ SUNG ---
    
    /// <summary>
    /// Địa chỉ của người dùng.
    /// </summary>
    /// <example>123 Đường ABC, Quận 1, TP. HCM</example>
    [Required(ErrorMessage = "Địa chỉ không được để trống.")]
    [StringLength(255)]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Ngày sinh của người dùng.
    /// </summary>
    /// <example>2000-01-30</example>
    [Required(ErrorMessage = "Ngày sinh không được để trống.")]
    public DateTime Birthday { get; set; }

    /// <summary>
    /// Giới tính của người dùng.
    /// </summary>
    [Required(ErrorMessage = "Giới tính không được để trống.")]
    [EnumDataType(typeof(Gender))]
    public Gender Gender { get; set; }
    
    // --- KẾT THÚC PHẦN BỔ SUNG ---

    /// <summary>
    /// Mật khẩu (tối thiểu 6 ký tự).
    /// </summary>
    /// <example>password123</example>
    [Required(ErrorMessage = "Mật khẩu không được để trống.")]
    [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Xác nhận mật khẩu.
    /// </summary>
    /// <example>password123</example>
    [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu.")]
    [Compare(nameof(Password), ErrorMessage = "Mật khẩu xác nhận không khớp.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
