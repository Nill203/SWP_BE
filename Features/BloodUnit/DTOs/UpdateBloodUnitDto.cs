using System.ComponentModel.DataAnnotations;
using BloodDonationBE.Common.Enums;

namespace BloodDonationBE.Features.BloodUnits.DTOs;

/// <summary>
/// DTO được sử dụng khi nhân viên/admin cập nhật trạng thái của một đơn vị máu.
/// </summary>
public class UpdateBloodUnitDto
{
    [Required]
    [EnumDataType(typeof(BloodUnitStatus))]
    public BloodUnitStatus Status { get; set; }

    public int? VerifiedByUserId { get; set; }
    
    public int? HospitalId { get; set; }
}
