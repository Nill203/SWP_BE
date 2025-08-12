using System.ComponentModel.DataAnnotations;
using BloodDonationBE.Common.Enums;

namespace BloodDonationBE.Features.BloodRequests.DTOs;

public class UpdateRequestStatusDto
{
    [Required]
    [EnumDataType(typeof(BloodRequestStatus))]
    public BloodRequestStatus Status { get; set; }
}
