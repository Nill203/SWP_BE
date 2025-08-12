using System.ComponentModel.DataAnnotations;
using BloodDonationBE.Common.Enums;
using BloodDonationBE.ValidationAttributes;

namespace BloodDonationBE.Features.CampaignRegistrations.DTOs;

public class UpdateRegistrationDto
{
    [Required(ErrorMessage = "status không được để trống")]
    [EnumDataType(typeof(RegistrationStatus), ErrorMessage = "Trạng thái không hợp lệ")]
    public RegistrationStatus Status { get; set; }
    
    [StringLength(255)]
    public string? Note { get; set; }


    [Range(100, 1000, ErrorMessage = "Lượng máu hiến phải từ 100ml đến 1000ml")]
    [RequiredIf(nameof(Status), RegistrationStatus.Completed, ErrorMessage = "Lượng máu không được để trống khi trạng thái là COMPLETED")]
    public int? Volume { get; set; }

    [EnumDataType(typeof(BloodType), ErrorMessage = "Nhóm máu không hợp lệ")]
    public BloodType? BloodType { get; set; }
}
