using System.ComponentModel.DataAnnotations;
using BloodDonationBE.Common.Enums;

namespace BloodDonationBE.Features.BloodUnits.DTOs;

/// <summary>
/// DTO được sử dụng khi Admin tạo một đơn vị máu thủ công.
/// </summary>
public class ManualCreateBloodUnitDto
{
    [Required]
    public int DonorId { get; set; }

    [Required]
    public int HospitalId { get; set; }

    [Required]
    [EnumDataType(typeof(BloodType))]
    public BloodType BloodType { get; set; }

    [Required]
    [Range(100, 1000, ErrorMessage = "Dung tích phải từ 100ml đến 1000ml")]
    public int Volume { get; set; }

    [Required]
    public DateTime DonationDate { get; set; }

    [EnumDataType(typeof(BloodProductType))]
    public BloodProductType? ProductType { get; set; }
}
