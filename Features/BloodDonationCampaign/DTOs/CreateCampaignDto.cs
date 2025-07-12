using System.ComponentModel.DataAnnotations;

namespace BloodDonationBE.Features.BloodDonationCampaigns.DTOs;

public class CreateCampaignDto
{
    /// <summary>
    /// Tên của sự kiện hiến máu.
    /// </summary>
    /// <example>Ngày hội Chủ Nhật Đỏ 2025</example>
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Địa điểm tổ chức sự kiện.
    /// </summary>
    /// <example>Trường Đại học Bách Khoa TP.HCM</example>
    [Required]
    [StringLength(255)]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Ngày diễn ra sự kiện.
    /// </summary>
    /// <example>2025-08-10</example>
    [Required]
    public DateTime ActiveTime { get; set; }

    /// <summary>
    /// Các khung giờ có thể hiến máu, dưới dạng một chuỗi JSON.
    /// </summary>
    /// <example>"[{\"start\":\"08:00\",\"end\":\"11:00\"},{\"start\":\"13:00\",\"end\":\"16:00\"}]"</example>
    [Required]
    public string DonateTime { get; set; } = string.Empty;

    /// <summary>
    /// Số lượng người đăng ký tối đa.
    /// </summary>
    /// <example>500</example>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng tối đa phải lớn hơn 0.")]
    public int Max { get; set; }

    /// <summary>
    /// ID của bệnh viện phụ trách.
    /// </summary>
    /// <example>1</example>
    [Required]
    public int HospitalId { get; set; }

    /// <summary>
    /// Vĩ độ của địa điểm (không bắt buộc).
    /// </summary>
    [Range(-90, 90)]
    public decimal? Lat { get; set; }

    /// <summary>
    /// Kinh độ của địa điểm (không bắt buộc).
    /// </summary>
    [Range(-180, 180)]
    public decimal? Lng { get; set; }
}
