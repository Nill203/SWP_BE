using System.ComponentModel.DataAnnotations;

namespace BloodDonationBE.Features.BloodDonationCampaigns.DTOs;

public class CreateCampaignDto
{
    /// Tên của sự kiện hiến máu.
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    /// Địa điểm tổ chức sự kiện.
    [Required]
    [StringLength(255)]
    public string Address { get; set; } = string.Empty;

    /// Ngày diễn ra sự kiện.
    [Required]
    public DateTime ActiveTime { get; set; }

    /// Các khung giờ có thể hiến máu.
    [Required]
    public string DonateTime { get; set; } = string.Empty;

    /// Số lượng người đăng ký tối đa.
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng tối đa phải lớn hơn 0.")]
    public int Max { get; set; }

    /// ID của bệnh viện phụ trách.
    [Required]
    public int HospitalId { get; set; }

    /// Vĩ độ của địa điểm.
    [Range(-90, 90)]
    public decimal? Lat { get; set; }

    /// Kinh độ của địa điểm.
    [Range(-180, 180)]
    public decimal? Lng { get; set; }
}
