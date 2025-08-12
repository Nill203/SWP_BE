using System.ComponentModel.DataAnnotations;
using BloodDonationBE.Common.Enums;

namespace BloodDonationBE.Features.Users.DTOs;

public class AdminCreateUserDto
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; }

    [Required]
    [Phone] 
    public string PhoneNumber { get; set; }

    [EmailAddress] 
    public string? Email { get; set; }

    [Required]
    public DateTime Birthday { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [Required]
    [StringLength(255)]
    public string Address { get; set; }

    public BloodType? BloodType { get; set; }

    [Range(-90, 90)]
    public decimal? Lat { get; set; }

    [Range(-180, 180)]
    public decimal? Lng { get; set; }
}