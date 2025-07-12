using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BloodDonationBE.Common.Enums;
using BloodDonationBE.Features.Hospitals;
using BloodDonationBE.Features.Users;

namespace BloodDonationBE.Features.BloodRequests;

[Table("blood_requests")]
public class BloodRequest
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string PatientName { get; set; } = string.Empty;

    [Required]
    public BloodType BloodType { get; set; }

    [Required]
    public BloodProductType ProductType { get; set; }

    [Required]
    [Range(1, 100, ErrorMessage = "Số lượng yêu cầu phải từ 1 đến 100 đơn vị.")]
    public int Quantity { get; set; }

    [Required]
    public string Reason { get; set; } = string.Empty;

    [Required]
    public BloodRequestStatus Status { get; set; } = BloodRequestStatus.Pending;

    // Timestamps for tracking
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? VerifiedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? FulfilledAt { get; set; }

    // --- Foreign Keys và Navigation Properties ---

    [Required]
    public int HospitalId { get; set; }
    [ForeignKey(nameof(HospitalId))]
    public Hospital Hospital { get; set; } = null!;

    [Required]
    public int RequestingUserId { get; set; }
    [ForeignKey(nameof(RequestingUserId))]
    public User RequestingUser { get; set; } = null!;

    public int? VerifyingStaffId { get; set; }
    [ForeignKey(nameof(VerifyingStaffId))]
    public User? VerifyingStaff { get; set; }

    public int? ApprovingAdminId { get; set; }
    [ForeignKey(nameof(ApprovingAdminId))]
    public User? ApprovingAdmin { get; set; }
}
