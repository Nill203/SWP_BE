using System.ComponentModel.DataAnnotations;
using BloodDonationBE.Common.Enums;

namespace BloodDonationBE.Features.Users.DTOs;

public class CreateStaffDto
{
    [Required]
    public string FullName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có đúng 10 ký tự.")]
    public string PhoneNumber { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    [Range(typeof(UserRole), nameof(UserRole.Staff), nameof(UserRole.Admin), ErrorMessage = "Vai trò chỉ có thể là Staff hoặc Admin.")]
    public UserRole Role { get; set; }
    public string? Address { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public BloodType? BloodType { get; set; }

    [Range(-90, 90)]
    public decimal? Lat { get; set; }

    [Range(-180, 180)]
    public decimal? Lng { get; set; }
}