using BloodDonationBE.Common.Enums;
using BloodDonationBE.Features.BloodRequests;

namespace BloodDonationBE.Features.BloodRequests.DTOs;

// DTO phụ để chứa thông tin tóm tắt
public class RequestUserDto
{
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
}

public class RequestHospitalDto
{
    public int HospitalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}


/// <summary>
/// DTO chính để trả về thông tin chi tiết của một yêu cầu máu.
/// </summary>
public class RequestResponseDto
{
    public int Id { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public BloodType BloodType { get; set; }
    public BloodProductType ProductType { get; set; }
    public int Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
    public BloodRequestStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? FulfilledAt { get; set; }

    // Thông tin từ các bảng liên quan
    public RequestHospitalDto Hospital { get; set; } = null!;
    public RequestUserDto RequestingUser { get; set; } = null!;
    public RequestUserDto? VerifyingStaff { get; set; }
    public RequestUserDto? ApprovingAdmin { get; set; }

    /// <summary>
    /// Phương thức tĩnh để tạo một DTO từ một entity BloodRequest.
    /// </summary>
    public static RequestResponseDto FromEntity(BloodRequest request)
    {
        return new RequestResponseDto
        {
            Id = request.Id,
            PatientName = request.PatientName,
            BloodType = request.BloodType,
            ProductType = request.ProductType,
            Quantity = request.Quantity,
            Reason = request.Reason,
            Status = request.Status,
            CreatedAt = request.CreatedAt,
            VerifiedAt = request.VerifiedAt,
            ApprovedAt = request.ApprovedAt,
            FulfilledAt = request.FulfilledAt,
            Hospital = new RequestHospitalDto
            {
                HospitalId = request.Hospital.Id,
                Name = request.Hospital.Name,
                Address = request.Hospital.Address
            },
            RequestingUser = new RequestUserDto
            {
                UserId = request.RequestingUser.UserId,
                FullName = request.RequestingUser.FullName
            },
            VerifyingStaff = request.VerifyingStaff != null
                ? new RequestUserDto { UserId = request.VerifyingStaff.UserId, FullName = request.VerifyingStaff.FullName }
                : null,
            ApprovingAdmin = request.ApprovingAdmin != null
                ? new RequestUserDto { UserId = request.ApprovingAdmin.UserId, FullName = request.ApprovingAdmin.FullName }
                : null
        };
    }
}
