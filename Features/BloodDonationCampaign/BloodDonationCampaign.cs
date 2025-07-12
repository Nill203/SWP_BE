using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BloodDonationBE.Features.CampaignRegistrations;
using BloodDonationBE.Features.Hospitals;
// using BloodDonationBE.Features.Users; // <-- XÓA DÒNG NÀY VÌ KHÔNG CẦN THIẾT

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

    /// <summary>
    /// Lưu trữ các khung giờ có thể hiến máu dưới dạng một chuỗi JSON.
    /// Ví dụ: "[{\"start\":\"08:00\",\"end\":\"11:00\"},{\"start\":\"13:00\",\"end\":\"16:00\"}]"
    /// </summary>
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

    // Mối quan hệ Nhiều-Một: Nhiều sự kiện thuộc về một bệnh viện
    [ForeignKey(nameof(HospitalId))]
    public Hospital Hospital { get; set; } = null!;

    // Mối quan hệ Một-Nhiều: Một sự kiện có nhiều lượt đăng ký
    public ICollection<CampaignRegistration> Registrations { get; set; } = new List<CampaignRegistration>();
}
