using BloodDonationBE.Features.BloodRequests.DTOs;

namespace BloodDonationBE.Features.BloodRequests;

public interface IBloodRequestService
{
    Task<RequestResponseDto> CreateRequestAsync(int requestingUserId, CreateRequestDto dto);
    Task<RequestResponseDto> VerifyRequestAsync(int requestId, int staffId, UpdateRequestStatusDto dto);
    Task<RequestResponseDto> ProcessRequestAsync(int requestId, int adminId, UpdateRequestStatusDto dto);
    Task<RequestResponseDto> FulfillRequestAsync(int requestId, int adminOrStaffId);
    Task<IEnumerable<RequestResponseDto>> GetAllRequestsAsync();
    Task<RequestResponseDto> GetRequestByIdAsync(int id);
    Task<IEnumerable<RequestResponseDto>> GetMyRequestsAsync(int userId);
    Task<RequestResponseDto> CancelRequestAsync(int requestId, int userId);
}
