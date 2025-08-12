using System.ComponentModel.DataAnnotations;
using BloodDonationBE.Common.Enums;

namespace BloodDonationBE.Features.Users.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "Họ và tên không được để trống.")]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email không được để trống.")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Số điện thoại không được để trống.")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có đúng 10 ký tự.")]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Địa chỉ không được để trống.")]
    [StringLength(255)]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ngày sinh không được để trống.")]
    public DateTime Birthday { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    [Required(ErrorMessage = "Giới tính không được để trống.")]
    [EnumDataType(typeof(Gender))]
    public Gender Gender { get; set; }

    // --- KẾT THÚC PHẦN BỔ SUNG ---

    [Required(ErrorMessage = "Mật khẩu không được để trống.")]
    [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu.")]
    [Compare(nameof(Password), ErrorMessage = "Mật khẩu xác nhận không khớp.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
