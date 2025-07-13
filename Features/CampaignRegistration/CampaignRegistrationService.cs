using Microsoft.EntityFrameworkCore;
using BloodDonationBE.Data;
using BloodDonationBE.Features.CampaignRegistrations.DTOs;
using BloodDonationBE.Common.Enums;
using BloodDonationBE.Features.BloodUnits;
using BloodDonationBE.Features.Users;

namespace BloodDonationBE.Features.CampaignRegistrations;

public class CampaignRegistrationService : ICampaignRegistrationService
{
    private readonly AppDbContext _context;
    private readonly IBloodUnitService _bloodUnitService;

    public CampaignRegistrationService(AppDbContext context, IBloodUnitService bloodUnitService)
    {
        _context = context;
        _bloodUnitService = bloodUnitService;
    }

    public async Task<RegistrationResponseDto> CreateRegistrationAsync(int userId, CreateRegistrationDto dto)
    {
        var isAlreadyRegistered = await _context.CampaignRegistrations
            .AnyAsync(r => r.UserId == userId && r.CampaignId == dto.CampaignId);

        if (isAlreadyRegistered)
        {
            throw new BadHttpRequestException("Bạn đã đăng ký tham gia sự kiện này rồi.");
        }

        var newRegistration = new CampaignRegistration
        {
            UserId = userId,
            CampaignId = dto.CampaignId,
            Note = dto.Note,
            Status = RegistrationStatus.Confirmed,
            RegisteredAt = DateTime.UtcNow,
            ProductType = dto.ProductType ?? BloodProductType.WholeBlood // <-- THÊM LOGIC NÀY
        };

        await _context.CampaignRegistrations.AddAsync(newRegistration);
        await _context.SaveChangesAsync();

        var created = await _context.CampaignRegistrations
            .Include(r => r.User)
            .Include(r => r.Campaign)
            .FirstAsync(r => r.Id == newRegistration.Id);

        return RegistrationResponseDto.FromEntity(created);
    }

    public async Task<IEnumerable<RegistrationResponseDto>> GetAllRegistrationsAsync()
    {
        var registrations = await _context.CampaignRegistrations
            .Include(r => r.User)
            .Include(r => r.Campaign)
            .ToListAsync();

        return registrations.Select(RegistrationResponseDto.FromEntity);
    }

    public async Task<IEnumerable<RegistrationResponseDto>> GetRegistrationsByCampaignIdAsync(int campaignId)
    {
        var registrations = await _context.CampaignRegistrations
            .Where(r => r.CampaignId == campaignId)
            .Include(r => r.User)
            .Include(r => r.Campaign)
            .ToListAsync();

        return registrations.Select(RegistrationResponseDto.FromEntity);
    }

    public async Task<RegistrationResponseDto> UpdateRegistrationAsync(int registrationId, UpdateRegistrationDto dto)
    {
        var registration = await _context.CampaignRegistrations
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == registrationId);

        if (registration == null)
        {
            throw new KeyNotFoundException("Không tìm thấy lượt đăng ký.");
        }

        registration.Status = dto.Status;
        if (dto.Note != null)
        {
            registration.Note = dto.Note;
        }

        // Luôn cập nhật nhóm máu nếu được cung cấp
        if (dto.BloodType.HasValue)
        {
            registration.User.BloodType = dto.BloodType.Value;
            _context.Users.Update(registration.User); // Chỉ thị rõ ràng cho EF Core rằng User đã được sửa đổi
        }

        if (dto.Status == RegistrationStatus.Completed)
        {
            if (!dto.Volume.HasValue)
            {
                throw new BadHttpRequestException("Lượng máu không được để trống khi trạng thái là COMPLETED.");
            }
            registration.Volume = dto.Volume.Value;

            // Gọi service để tạo đơn vị máu
            await _bloodUnitService.CreateFromRegistrationAsync(registration.Id);
        }

        await _context.SaveChangesAsync();

        // Load lại campaign để đảm bảo dữ liệu mới nhất
        await _context.Entry(registration).Reference(r => r.Campaign).LoadAsync();

        return RegistrationResponseDto.FromEntity(registration);
    }

    public async Task<IEnumerable<DonationHistoryResponseDto>> GetDonationHistoryByUserIdAsync(int userId)
    {
        return await _context.CampaignRegistrations
            .Where(r => r.UserId == userId && r.Status == RegistrationStatus.Completed && r.Volume.HasValue)
            .Include(r => r.Campaign)
            .OrderByDescending(r => r.Campaign.ActiveTime)
            .Select(r => new DonationHistoryResponseDto
            {
                Date = r.Campaign.ActiveTime.ToString("dd/MM/yyyy"),
                Volume = r.Volume!.Value,
                Location = r.Campaign.Address
            })
            .ToListAsync();
    }
    
    public async Task<IEnumerable<RegistrationResponseDto>> GetMyRegistrationsAsync(int userId)
    {
        var registrations = await _context.CampaignRegistrations
            .Where(r => r.UserId == userId)
            .Include(r => r.User)
            .Include(r => r.Campaign)
            .OrderByDescending(r => r.Campaign.ActiveTime)
            .ToListAsync();

        return registrations.Select(RegistrationResponseDto.FromEntity);
    }

    public async Task<RegistrationResponseDto> CancelRegistrationAsync(int registrationId, int userId)
    {
        var registration = await _context.CampaignRegistrations
            .Include(r => r.User)
            .Include(r => r.Campaign)
            .FirstOrDefaultAsync(r => r.Id == registrationId);

        if (registration == null)
        {
            throw new KeyNotFoundException("Không tìm thấy lượt đăng ký.");
        }

        // Kiểm tra bảo mật: Đảm bảo người dùng chỉ có thể hủy đăng ký của chính mình
        if (registration.UserId != userId)
        {
            throw new UnauthorizedAccessException("Bạn không có quyền hủy lượt đăng ký này.");
        }

        // Kiểm tra xem có thể hủy được không (ví dụ: không thể hủy khi đã hoàn thành)
        if (registration.Status == RegistrationStatus.Completed || registration.Status == RegistrationStatus.Cancelled)
        {
            throw new BadHttpRequestException($"Không thể hủy lượt đăng ký ở trạng thái '{registration.Status}'.");
        }

        registration.Status = RegistrationStatus.Cancelled;
        registration.Note = "Người dùng tự hủy.";

        await _context.SaveChangesAsync();
        return RegistrationResponseDto.FromEntity(registration);
    }
}
