using System.ComponentModel.DataAnnotations;
using BloodDonationBE.Common.Enums;

namespace BloodDonationBE.Features.BloodUnits.DTOs;

public class UpdateBloodUnitDto
{
    [Required]
    [EnumDataType(typeof(BloodUnitStatus))]
    public BloodUnitStatus Status { get; set; }

    public int? VerifiedByUserId { get; set; }
    
    public int? HospitalId { get; set; }
}
