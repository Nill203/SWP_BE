using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BloodDonationBE.Common.Enums;

namespace BloodDonationBE.Features.Users;

[Table("user")]
public class User
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Required]
    [StringLength(255)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    [Column("full_name")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [Column("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [Column("blood_type")]
    public BloodType BloodType { get; set; }

    [Required]
    public UserRole Role { get; set; }

    [StringLength(255)]
    public string? Address { get; set; }

    public Gender? Gender { get; set; }

    [Column(TypeName = "date")]
    public DateTime? Birthday { get; set; }

    [Column("avatar_image")]
    public string? AvatarImage { get; set; }

    [Column("lat", TypeName = "decimal(10, 8)")]
    public decimal? Lat { get; set; }

    [Column("lng", TypeName = "decimal(11, 8)")]
    public decimal? Lng { get; set; }

    public bool IsVerified { get; set; } = false;
    public string? VerificationToken { get; set; }

    // --- CÁC TRƯỜNG MỚI ĐƯỢC BỔ SUNG ---

    /// <summary>
    /// Trạng thái sẵn sàng hiến máu của người dùng.
    /// </summary>
    [Required]
    public AvailabilityStatus AvailabilityStatus { get; set; } = AvailabilityStatus.Available;

    /// <summary>
    /// Ngày hiến máu cuối cùng của người dùng.
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? LastDonationDate { get; set; }
}
