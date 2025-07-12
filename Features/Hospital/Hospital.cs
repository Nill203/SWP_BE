using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// Thêm bí danh (alias) để giải quyết xung đột tên
using BloodDonationBE.Features.BloodDonationCampaigns;

namespace BloodDonationBE.Features.Hospitals;

[Table("hospitals")]
[Index(nameof(Name), IsUnique = true)] // Tạo index để đảm bảo tên bệnh viện là duy nhất
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

    // Mối quan hệ Một-Nhiều: Một bệnh viện có thể tổ chức nhiều sự kiện hiến máu
    // Giờ đây, trình biên dịch sẽ hiểu đúng BloodDonationCampaign là một lớp
    public ICollection<BloodDonationCampaign> Campaigns { get; set; } = new List<BloodDonationCampaign>();
}
