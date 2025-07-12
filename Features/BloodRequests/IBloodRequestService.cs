using BloodDonationBE.Features.BloodRequests.DTOs;

namespace BloodDonationBE.Features.BloodRequests;

public interface IBloodRequestService
{
    Task<RequestResponseDto> CreateRequestAsync(int requestingUserId, CreateRequestDto dto);

    Task<RequestResponseDto> VerifyRequestAsync(int requestId, int staffId, UpdateRequestStatusDto dto);

    Task<RequestResponseDto> ProcessRequestAsync(int requestId, int adminId, UpdateRequestStatusDto dto);

    // ==> PHƯƠNG THỨC MỚI
    /// <summary>
    /// Hoàn thành một yêu cầu đã được phê duyệt (Approved), cập nhật kho máu.
    /// </summary>
    Task<RequestResponseDto> FulfillRequestAsync(int requestId, int adminOrStaffId);

    Task<IEnumerable<RequestResponseDto>> GetAllRequestsAsync();

    Task<RequestResponseDto> GetRequestByIdAsync(int id);
}
