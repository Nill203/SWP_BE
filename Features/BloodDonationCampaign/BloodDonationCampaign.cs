using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BloodDonationBE.Features.CampaignRegistrations;
using BloodDonationBE.Features.Hospitals;

namespace BloodDonationBE.Features.BloodDonationCampaigns;

[Table("blood_donation_campaign")]
public class BloodDonationCampaign
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string Address { get; set; } = "Vinhomes Grand Park, Long Bình, Thủ Đức, HCM";

    [Column(TypeName = "date")]
    public DateTime ActiveTime { get; set; }

    [Column(TypeName = "json")]
    public string DonateTime { get; set; } = string.Empty;

    [Required]
    public int Max { get; set; } = 0;

    [Column("lat", TypeName = "decimal(10, 8)")]
    public decimal? Lat { get; set; }

    [Column("lng", TypeName = "decimal(11, 8)")]
    public decimal? Lng { get; set; }

    // --- Foreign Key và Navigation Properties ---

    [Required]
    public int HospitalId { get; set; }

    [ForeignKey(nameof(HospitalId))]
    public Hospital Hospital { get; set; } = null!;

    public ICollection<CampaignRegistration> Registrations { get; set; } = new List<CampaignRegistration>();
}
