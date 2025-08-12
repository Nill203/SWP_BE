using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BloodDonationBE.Common.Enums;
using BloodDonationBE.Features.BloodDonationCampaigns;
using BloodDonationBE.Features.BloodUnits;
using BloodDonationBE.Features.Users;

namespace BloodDonationBE.Features.CampaignRegistrations;

[Table("campaign_registration")]
public class CampaignRegistration
{
    [Key]
    public int Id { get; set; }

    [Required]
    public RegistrationStatus Status { get; set; } = RegistrationStatus.Confirmed;

    [StringLength(255)]
    public string? Note { get; set; }

    public int? Volume { get; set; }

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    [Required]
    public BloodProductType ProductType { get; set; } = BloodProductType.WholeBlood; // <-- THÊM TRƯỜDNG MỚI


    [Required]
    public int CampaignId { get; set; }

    [ForeignKey(nameof(CampaignId))]
    public BloodDonationCampaign Campaign { get; set; } = null!;

    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    public virtual ICollection<BloodUnit> BloodUnits { get; set; } = new List<BloodUnit>();
}

