using System.ComponentModel.DataAnnotations;
using BloodDonationBE.Common.Enums;

namespace BloodDonationBE.Features.Users.DTOs;

public class AdminCreateUserDto
{
    /// <summary>
    /// Họ và tên đầy đủ của người dùng.
    /// </summary>
    /// <example>Nguyễn Văn B</example>
    [Required]
    [StringLength(100)]
    public string FullName { get; set; }

    /// <summary>
    /// Số điện thoại của người dùng.
    /// </summary>
    /// <example>0987654321</example>
    [Required]
    [Phone] // Sử dụng attribute [Phone] để validate số điện thoại
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Địa chỉ email của người dùng (không bắt buộc).
    /// </summary>
    /// <example>nguyen.van.b@example.com</example>
    [EmailAddress] // Vẫn validate nếu giá trị được cung cấp
    public string? Email { get; set; }

    /// <summary>
    /// Ngày sinh của người dùng.
    /// </summary>
    /// <example>1995-08-15</example>
    [Required]
    public DateTime Birthday { get; set; }

    /// <summary>
    /// Giới tính của người dùng.
    /// </summary>
    /// <example>Male</example>
    [Required]
    public Gender Gender { get; set; }

    /// <summary>
    /// Địa chỉ của người dùng.
    /// </summary>
    /// <example>123 Đường ABC, Quận 1, TP. HCM</example>
    [Required]
    [StringLength(255)]
    public string Address { get; set; }

    /// <summary>
    /// Nhóm máu của người dùng (không bắt buộc).
    /// </summary>
    /// <example>A_POS</example>
    public BloodType? BloodType { get; set; }

    [Range(-90, 90)]
    public decimal? Lat { get; set; }

    [Range(-180, 180)]
    public decimal? Lng { get; set; }
}