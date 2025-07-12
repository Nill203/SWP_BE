using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BloodDonationBE.Common.Enums;
using BloodDonationBE.Features.CampaignRegistrations;
using BloodDonationBE.Features.Hospitals;
using BloodDonationBE.Features.Users;

// Đặt namespace là số nhiều để nhất quán
namespace BloodDonationBE.Features.BloodUnits;

[Table("blood_units")]
public class BloodUnit
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column("blood_type")]
    public BloodType BloodType { get; set; }

    [Required]
    public int Volume { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateTime DonationDate { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateTime ExpiryDate { get; set; }

    [Required]
    public BloodUnitStatus Status { get; set; } = BloodUnitStatus.AwaitingTesting;

    [Required]
    [Column("product_type")]
    public BloodProductType ProductType { get; set; } = BloodProductType.WholeBlood;

    public DateTime? VerificationDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime? IssueDate { get; set; }

    // --- Foreign Keys và Navigation Properties ---

    public int? RegistrationId { get; set; }
    [ForeignKey(nameof(RegistrationId))]
    public CampaignRegistration? Registration { get; set; }

    [Required]
    public int HospitalId { get; set; }
    [ForeignKey(nameof(HospitalId))]
    public Hospital Hospital { get; set; } = null!;

    public int? DonorId { get; set; }
    [ForeignKey(nameof(DonorId))]
    public User? Donor { get; set; }

    public int? VerifiedByUserId { get; set; }
    [ForeignKey(nameof(VerifiedByUserId))]
    public User? Verifier { get; set; }
}
