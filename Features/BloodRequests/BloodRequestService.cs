using Microsoft.EntityFrameworkCore;
using BloodDonationBE.Data;
using BloodDonationBE.Features.BloodRequests.DTOs;
using BloodDonationBE.Common.Enums;
using BloodDonationBE.Features.Users;

namespace BloodDonationBE.Features.BloodRequests;

public class BloodRequestService : IBloodRequestService
{
    private readonly AppDbContext _context;

    public BloodRequestService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RequestResponseDto> CreateRequestAsync(int requestingUserId, CreateRequestDto dto)
    {
        var request = new BloodRequest
        {
            PatientName = dto.PatientName,
            BloodType = dto.BloodType,
            ProductType = dto.ProductType,
            Quantity = dto.Quantity,
            Reason = dto.Reason,
            Status = BloodRequestStatus.Pending,
            HospitalId = dto.HospitalId,
            RequestingUserId = requestingUserId,
            CreatedAt = DateTime.UtcNow
        };

        await _context.BloodRequests.AddAsync(request);
        await _context.SaveChangesAsync();
        return await GetRequestByIdAsync(request.Id);
    }

    public async Task<RequestResponseDto> VerifyRequestAsync(int requestId, int staffId, UpdateRequestStatusDto dto)
    {
        var request = await FindRequestEntityByIdAsync(requestId);
        if (request.Status != BloodRequestStatus.Pending)
        {
            throw new BadHttpRequestException($"Chỉ có thể xác thực yêu cầu ở trạng thái 'Pending'.");
        }
        if (dto.Status != BloodRequestStatus.Verified && dto.Status != BloodRequestStatus.Rejected)
        {
            throw new BadHttpRequestException("Nhân viên chỉ có thể cập nhật trạng thái thành 'Verified' hoặc 'Rejected'.");
        }

        request.Status = dto.Status;
        request.VerifyingStaffId = staffId;
        request.VerifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return RequestResponseDto.FromEntity(request);
    }

    public async Task<RequestResponseDto> ProcessRequestAsync(int requestId, int adminId, UpdateRequestStatusDto dto)
    {
        var request = await FindRequestEntityByIdAsync(requestId);
        if (request.Status != BloodRequestStatus.Verified)
        {
            throw new BadHttpRequestException($"Chỉ có thể xử lý yêu cầu ở trạng thái 'Verified'.");
        }
        if (dto.Status != BloodRequestStatus.Approved)
        {
             throw new BadHttpRequestException("Admin chỉ có thể cập nhật trạng thái thành 'Approved' ở bước này.");
        }

        // Kiểm tra kho máu
        var availableUnitsCount = await _context.BloodUnits
            .CountAsync(bu => bu.Status == BloodUnitStatus.InStock &&
                                bu.BloodType == request.BloodType &&
                                bu.ProductType == request.ProductType);

        if (availableUnitsCount < request.Quantity)
        {
            request.Status = BloodRequestStatus.PendingAppeal; // Không đủ máu, chuyển sang chờ kêu gọi
        }
        else
        {
            request.Status = BloodRequestStatus.Approved; // Đủ máu, phê duyệt
        }

        request.ApprovingAdminId = adminId;
        request.ApprovedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return RequestResponseDto.FromEntity(request);
    }

    public async Task<RequestResponseDto> FulfillRequestAsync(int requestId, int adminOrStaffId)
    {
        var request = await FindRequestEntityByIdAsync(requestId);
        if (request.Status != BloodRequestStatus.Approved)
        {
            throw new BadHttpRequestException($"Chỉ có thể hoàn thành yêu cầu ở trạng thái 'Approved'.");
        }

        var unitsToFulfill = await _context.BloodUnits
            .Where(bu => bu.Status == BloodUnitStatus.InStock &&
                            bu.BloodType == request.BloodType &&
                            bu.ProductType == request.ProductType)
            .Take(request.Quantity)
            .ToListAsync();

        if (unitsToFulfill.Count < request.Quantity)
        {
            // Trường hợp hy hữu: kho đã thay đổi sau khi duyệt.
            throw new BadHttpRequestException("Số lượng máu trong kho không còn đủ để hoàn thành yêu cầu.");
        }

        foreach (var unit in unitsToFulfill)
        {
            unit.Status = BloodUnitStatus.Used;
            unit.IssueDate = DateTime.UtcNow;
        }

        request.Status = BloodRequestStatus.Fulfilled;
        request.FulfilledAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return RequestResponseDto.FromEntity(request);
    }

    public async Task<IEnumerable<RequestResponseDto>> GetAllRequestsAsync()
    {
        var requests = await _context.BloodRequests
            .Include(r => r.Hospital).Include(r => r.RequestingUser)
            .Include(r => r.VerifyingStaff).Include(r => r.ApprovingAdmin)
            .ToListAsync();
        return requests.Select(RequestResponseDto.FromEntity);
    }

    public async Task<RequestResponseDto> GetRequestByIdAsync(int id)
    {
        var request = await FindRequestEntityByIdAsync(id);
        return RequestResponseDto.FromEntity(request);
    }

    private async Task<BloodRequest> FindRequestEntityByIdAsync(int id)
    {
        var request = await _context.BloodRequests
            .Include(r => r.Hospital).Include(r => r.RequestingUser)
            .Include(r => r.VerifyingStaff).Include(r => r.ApprovingAdmin)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (request == null)
        {
            throw new KeyNotFoundException($"Không tìm thấy yêu cầu máu với ID: {id}");
        }
        return request;
    }
}
