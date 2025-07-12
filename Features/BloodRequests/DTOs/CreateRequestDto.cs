using System.ComponentModel.DataAnnotations;
using BloodDonationBE.Common.Enums;

namespace BloodDonationBE.Features.BloodRequests.DTOs;

/// <summary>
/// DTO được sử dụng khi người dùng công khai tạo một yêu cầu máu khẩn cấp.
/// </summary>
public class CreateRequestDto
{
    /// <summary>
    /// Tên của bệnh nhân cần máu.
    /// </summary>
    /// <example>Nguyễn Thị Lan</example>
    [Required]
    [StringLength(100)]
    public string PatientName { get; set; } = string.Empty;

    /// <summary>
    /// Nhóm máu cần thiết.
    /// </summary>
    [Required]
    [EnumDataType(typeof(BloodType))]
    public BloodType BloodType { get; set; }

    /// <summary>
    /// Loại sản phẩm máu cần thiết.
    /// </summary>
    [Required]
    [EnumDataType(typeof(BloodProductType))]
    public BloodProductType ProductType { get; set; }

    /// <summary>
    /// Số lượng đơn vị máu cần (mỗi đơn vị khoảng 350ml).
    /// </summary>
    /// <example>2</example>
    [Required]
    [Range(1, 100)]
    public int Quantity { get; set; }

    /// <summary>
    /// Lý do hoặc câu chuyện kêu gọi (để tăng tính thuyết phục).
    /// </summary>
    /// <example>Bệnh nhân cần phẫu thuật tim gấp.</example>
    [Required]
    [StringLength(1000)]
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// ID của bệnh viện nơi bệnh nhân đang điều trị.
    /// </summary>
    /// <example>1</example>
    [Required]
    public int HospitalId { get; set; }
}
