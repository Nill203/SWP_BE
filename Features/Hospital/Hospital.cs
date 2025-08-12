using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// Thêm bí danh (alias) để giải quyết xung đột tên
using BloodDonationBE.Features.BloodDonationCampaigns;

namespace BloodDonationBE.Features.Hospitals;

[Table("hospitals")]
[Index(nameof(Name), IsUnique = true)]
public class Hospital
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string Address { get; set; } = string.Empty;

    [StringLength(255)]
    [Column("contact_info")]
    public string? ContactInfo { get; set; }

    public ICollection<BloodDonationCampaign> Campaigns { get; set; } = new List<BloodDonationCampaign>();
}
