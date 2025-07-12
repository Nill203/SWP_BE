using System.ComponentModel.DataAnnotations;
using BloodDonationBE.Common.Enums;

namespace BloodDonationBE.Features.Users.DTOs;

public class UserUpdateDto // Đổi tên thành UserUpdateDto để rõ nghĩa hơn
{
    /// <summary>
    /// Mật khẩu mới (tối thiểu 6 ký tự).
    /// </summary>
    [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
    public string? Password { get; set; }

    /// <summary>
    /// Họ và tên đầy đủ.
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// Địa chỉ email.
    /// </summary>
    [EmailAddress]
    public string? Email { get; set; }

    /// <summary>
    /// Số điện thoại (đúng 10 ký tự).
    /// </summary>
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có đúng 10 ký tự.")]
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// Nhóm máu.
    /// </summary>
    public BloodType? BloodType { get; set; }

    /// <summary>
    /// Địa chỉ.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Giới tính.
    /// </summary>
    public Gender? Gender { get; set; }

    /// <summary>
    /// Ngày sinh (định dạng YYYY-MM-DD).
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    // Link ảnh đại diện.
    /// </summary>
    public string? AvatarImage { get; set; }

    /// <summary>
    /// Vĩ độ.
    /// </summary>
    [Range(-90, 90, ErrorMessage = "Vĩ độ không hợp lệ.")]
    public decimal? Lat { get; set; }

    /// <summary>
    /// Kinh độ.
    /// </summary>
    [Range(-180, 180, ErrorMessage = "Kinh độ không hợp lệ.")]
    public decimal? Lng { get; set; }
}