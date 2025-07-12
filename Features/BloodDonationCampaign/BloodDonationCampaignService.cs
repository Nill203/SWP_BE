using Microsoft.EntityFrameworkCore;
using BloodDonationBE.Data;
using BloodDonationBE.Features.BloodDonationCampaigns.DTOs;
using BloodDonationBE.Common.Enums;
using BloodDonationBE.Features.Hospitals; // <-- Sửa lại namespace cho Hospital

// ==> Đảm bảo namespace là số nhiều
namespace BloodDonationBE.Features.BloodDonationCampaigns;

public class BloodDonationCampaignService : IBloodDonationCampaignService
{
    private readonly AppDbContext _context;

    public BloodDonationCampaignService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CampaignResponseDto> CreateCampaignAsync(CreateCampaignDto dto)
    {
        var campaign = new BloodDonationCampaign
        {
            Name = dto.Name,
            Address = dto.Address,
            ActiveTime = dto.ActiveTime,
            DonateTime = dto.DonateTime,
            Max = dto.Max,
            HospitalId = dto.HospitalId,
            Lat = dto.Lat,
            Lng = dto.Lng
        };

        await _context.BloodDonationCampaigns.AddAsync(campaign);
        await _context.SaveChangesAsync();

        return await GetCampaignByIdAsync(campaign.Id);
    }

    public async Task<IEnumerable<CampaignResponseDto>> GetAllCampaignsAsync()
    {
        var campaigns = await _context.BloodDonationCampaigns
            .Include(c => c.Hospital)
            .Include(c => c.Registrations)
            .ToListAsync();

        return campaigns.Select(c => ToCampaignResponseDto(c));
    }

    public async Task<CampaignResponseDto> GetCampaignByIdAsync(int id)
    {
        var campaign = await _context.BloodDonationCampaigns
            .Include(c => c.Hospital)
            .Include(c => c.Registrations)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (campaign == null)
        {
            throw new KeyNotFoundException($"Không tìm thấy sự kiện với ID {id}");
        }

        return ToCampaignResponseDto(campaign);
    }

    public async Task<CampaignResponseDto> UpdateCampaignAsync(int id, UpdateCampaignDto dto)
    {
        var campaign = await _context.BloodDonationCampaigns.FindAsync(id);
        if (campaign == null)
        {
            throw new KeyNotFoundException($"Không tìm thấy sự kiện với ID {id}");
        }

        if (!string.IsNullOrEmpty(dto.Name)) campaign.Name = dto.Name;
        if (!string.IsNullOrEmpty(dto.Address)) campaign.Address = dto.Address;
        if (dto.ActiveTime.HasValue) campaign.ActiveTime = dto.ActiveTime.Value;
        if (!string.IsNullOrEmpty(dto.DonateTime)) campaign.DonateTime = dto.DonateTime;
        if (dto.Max.HasValue) campaign.Max = dto.Max.Value;
        if (dto.HospitalId.HasValue) campaign.HospitalId = dto.HospitalId.Value;
        if (dto.Lat.HasValue) campaign.Lat = dto.Lat.Value;
        if (dto.Lng.HasValue) campaign.Lng = dto.Lng.Value;

        await _context.SaveChangesAsync();
        return await GetCampaignByIdAsync(id);
    }

    public async Task DeleteCampaignAsync(int id)
    {
        var campaign = await _context.BloodDonationCampaigns.FindAsync(id);
        if (campaign == null)
        {
            throw new KeyNotFoundException($"Không tìm thấy sự kiện với ID {id}");
        }

        _context.BloodDonationCampaigns.Remove(campaign);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CampaignResponseDto>> SearchCampaignsByDateRangeAsync(DateTime? start, DateTime? end)
    {
        var query = _context.BloodDonationCampaigns
            .Include(c => c.Hospital)
            .Include(c => c.Registrations)
            .AsQueryable();

        if (start.HasValue)
        {
            query = query.Where(c => c.ActiveTime.Date >= start.Value.Date);
        }

        if (end.HasValue)
        {
            query = query.Where(c => c.ActiveTime.Date <= end.Value.Date);
        }

        var campaigns = await query.ToListAsync();
        return campaigns.Select(c => ToCampaignResponseDto(c));
    }

    // --- Private Helper Methods ---

    private CampaignResponseDto ToCampaignResponseDto(BloodDonationCampaign campaign)
    {
        return new CampaignResponseDto
        {
            Id = campaign.Id,
            Name = campaign.Name,
            Address = campaign.Address,
            ActiveTime = campaign.ActiveTime,
            DonateTime = campaign.DonateTime,
            Max = campaign.Max,
            Hospital = campaign.Hospital != null ? new CampaignHospitalDto { Id = campaign.Hospital.Id, Name = campaign.Hospital.Name } : null,
            RegisteredCount = campaign.Registrations?.Count ?? 0,
            Status = GetCampaignStatus(campaign.ActiveTime),
            Lat = campaign.Lat,
            Lng = campaign.Lng
        };
    }

    private CampaignStatus GetCampaignStatus(DateTime activeTime)
    {
        var today = DateTime.UtcNow.Date;
        var campaignDate = activeTime.Date;

        if (campaignDate > today) return CampaignStatus.Upcoming;
        if (campaignDate < today) return CampaignStatus.Finished;
        return CampaignStatus.Ongoing;
    }
}
