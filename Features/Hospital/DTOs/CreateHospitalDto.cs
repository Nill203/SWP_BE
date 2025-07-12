using System.ComponentModel.DataAnnotations;

namespace BloodDonationBE.Features.Hospitals.DTOs;

public class CreateHospitalDto
{
    /// <summary>
    /// Tên của bệnh viện. Phải là duy nhất.
    /// </summary>
    /// <example>Bệnh viện Truyền máu Huyết học</example>
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Địa chỉ của bệnh viện.
    /// </summary>
    /// <example>118 Hồng Bàng, Phường 12, Quận 5, TP.HCM</example>
    [Required]
    [StringLength(255)]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Thông tin liên hệ (SĐT, email, v.v.).
    /// </summary>
    /// <example>02839571100</example>
    [StringLength(255)]
    public string? ContactInfo { get; set; }
}
