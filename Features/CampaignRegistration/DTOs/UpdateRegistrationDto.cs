using System.ComponentModel.DataAnnotations;
using BloodDonationBE.Common.Enums;
using BloodDonationBE.ValidationAttributes; // Namespace cho custom validation

namespace BloodDonationBE.Features.CampaignRegistrations.DTOs;

public class UpdateRegistrationDto
{
    /// <summary>
    /// Trạng thái mới của lượt đăng ký.
    /// </summary>
    [Required(ErrorMessage = "status không được để trống")]
    [EnumDataType(typeof(RegistrationStatus), ErrorMessage = "Trạng thái không hợp lệ")]
    public RegistrationStatus Status { get; set; }
    
    /// <summary>
    /// Ghi chú của nhân viên khi cập nhật (không bắt buộc).
    /// </summary>
    [StringLength(255)]
    public string? Note { get; set; }

    /// <summary>
    /// Lượng máu đã hiến (ml). Bắt buộc khi status là COMPLETED.
    /// </summary>
    [Range(100, 1000, ErrorMessage = "Lượng máu hiến phải từ 100ml đến 1000ml")]
    [RequiredIf(nameof(Status), RegistrationStatus.Completed, ErrorMessage = "Lượng máu không được để trống khi trạng thái là COMPLETED")]
    public int? Volume { get; set; }

    /// <summary>
    /// Nhóm máu của người hiến (cần thiết nếu người dùng chưa có).
    /// </summary>
    [EnumDataType(typeof(BloodType), ErrorMessage = "Nhóm máu không hợp lệ")]
    public BloodType? BloodType { get; set; }
}
