using System.ComponentModel.DataAnnotations;
using BloodDonationBE.Common.Enums;

namespace BloodDonationBE.Features.BloodRequests.DTOs;

/// <summary>
/// DTO được sử dụng bởi nhân viên hoặc admin để cập nhật trạng thái của một yêu cầu máu.
/// </summary>
public class UpdateRequestStatusDto
{
    /// <summary>
    /// Trạng thái mới của yêu cầu.
    /// </summary>
    /// <example>Verified</example>
    [Required]
    [EnumDataType(typeof(BloodRequestStatus))]
    public BloodRequestStatus Status { get; set; }
}
