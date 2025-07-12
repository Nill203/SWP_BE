using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BloodDonationBE.Common.Enums;
using BloodDonationBE.Features.BloodDonationCampaigns;
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

    /// <summary>
    /// Lượng máu đã hiến (ml).
    /// </summary>
    public int? Volume { get; set; }

    /// <summary>
    /// Thời điểm đăng ký.
    /// </summary>
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Loại sản phẩm máu người dùng đăng ký hiến.
    /// </summary>
    [Required]
    public BloodProductType ProductType { get; set; } = BloodProductType.WholeBlood; // <-- THÊM TRƯỜDNG MỚI

    // --- Foreign Keys và Navigation Properties ---

    [Required]
    public int CampaignId { get; set; }

    [ForeignKey(nameof(CampaignId))]
    public BloodDonationCampaign Campaign { get; set; } = null!;

    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
}
