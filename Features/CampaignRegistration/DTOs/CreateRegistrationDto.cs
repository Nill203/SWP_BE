using System.ComponentModel.DataAnnotations;
using BloodDonationBE.Common.Enums; // <-- Thêm using cho BloodProductType

namespace BloodDonationBE.Features.CampaignRegistrations.DTOs;

public class CreateRegistrationDto
{
    /// <summary>
    /// ID của sự kiện hiến máu mà người dùng đăng ký.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "campaignId không được để trống")]
    public int CampaignId { get; set; }

    /// <summary>
    /// Ghi chú thêm của người dùng khi đăng ký (không bắt buộc).
    /// </summary>
    /// <example>Tôi có thể đến sau 5 giờ chiều.</example>
    [StringLength(255)]
    public string? Note { get; set; }

    /// <summary>
    /// Loại sản phẩm máu người dùng muốn hiến (không bắt buộc).
    /// Mặc định là Hiến máu toàn phần (WholeBlood).
    /// </summary>
    [EnumDataType(typeof(BloodProductType))]
    public BloodProductType? ProductType { get; set; } // <-- THÊM TRƯỜNG MỚI
}
