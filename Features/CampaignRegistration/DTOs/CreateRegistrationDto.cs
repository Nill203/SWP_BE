using System.ComponentModel.DataAnnotations;
using BloodDonationBE.Common.Enums; // <-- Thêm using cho BloodProductType

namespace BloodDonationBE.Features.CampaignRegistrations.DTOs;

public class CreateRegistrationDto
{
    [Required(ErrorMessage = "campaignId không được để trống")]
    public int CampaignId { get; set; }
    [StringLength(255)]
    public string? Note { get; set; }
    [EnumDataType(typeof(BloodProductType))]
    public BloodProductType? ProductType { get; set; }
}
