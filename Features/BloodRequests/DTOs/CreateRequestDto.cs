using System.ComponentModel.DataAnnotations;
using BloodDonationBE.Common.Enums;

namespace BloodDonationBE.Features.BloodRequests.DTOs;

public class CreateRequestDto
{
    [Required]
    [StringLength(100)]
    public string PatientName { get; set; } = string.Empty;

    [Required]
    [EnumDataType(typeof(BloodType))]
    public BloodType BloodType { get; set; }

    [Required]
    [EnumDataType(typeof(BloodProductType))]
    public BloodProductType ProductType { get; set; }

    [Required]
    [Range(1, 100)]
    public int Quantity { get; set; }

    [Required]
    [StringLength(1000)]
    public string Reason { get; set; } = string.Empty;

    [Required]
    public int HospitalId { get; set; }
}
