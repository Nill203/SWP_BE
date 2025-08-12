using Microsoft.EntityFrameworkCore;
using BloodDonationBE.Data;
using BloodDonationBE.Features.BloodUnits.DTOs;
using BloodDonationBE.Common.Enums;
using BloodDonationBE.Features.Users;

namespace BloodDonationBE.Features.BloodUnits;

public class BloodUnitService : IBloodUnitService
{
    private readonly AppDbContext _context;

    public BloodUnitService(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateFromRegistrationAsync(int registrationId)
    {
        var registration = await _context.CampaignRegistrations
            .Include(r => r.User)
            .Include(r => r.Campaign)
            .FirstOrDefaultAsync(r => r.Id == registrationId);

        if (registration == null || !registration.Volume.HasValue)
        {
            return;
        }

        var bloodUnit = new BloodUnit
        {
            BloodType = registration.User.BloodType,
            Volume = registration.Volume.Value,
            DonationDate = registration.Campaign.ActiveTime,
            ExpiryDate = CalculateExpiryDate(registration.Campaign.ActiveTime, registration.ProductType),
            Status = BloodUnitStatus.AwaitingTesting,
            ProductType = registration.ProductType,
            RegistrationId = registration.Id,
            HospitalId = registration.Campaign.HospitalId,
            DonorId = registration.UserId,
        };

        _context.BloodUnits.Add(bloodUnit);
        await _context.SaveChangesAsync();
    }

public async Task<BloodUnitResponseDto> CreateManualAsync(ManualCreateBloodUnitDto dto)
{
    var bloodUnit = new BloodUnit
    {
        DonorId = dto.DonorId,
        HospitalId = dto.HospitalId,
        BloodType = dto.BloodType,
        Volume = dto.Volume,
        DonationDate = dto.DonationDate,
        ProductType = dto.ProductType ?? BloodProductType.WholeBlood,
        ExpiryDate = CalculateExpiryDate(dto.DonationDate, dto.ProductType ?? BloodProductType.WholeBlood),
        
        Status = BloodUnitStatus.AwaitingTesting
    };

    await _context.BloodUnits.AddAsync(bloodUnit);
    await _context.SaveChangesAsync();

    return await GetByIdAsync(bloodUnit.Id);
}

    public async Task<BloodUnitResponseDto> UpdateStatusAsync(int id, UpdateBloodUnitDto dto, int verifierId)
    {
        var bloodUnit = await FindUnitEntityByIdAsync(id);

        bloodUnit.Status = dto.Status;

        if (dto.Status == BloodUnitStatus.InStock || dto.Status == BloodUnitStatus.TestingFailed)
        {
            bloodUnit.VerifiedByUserId = verifierId;
            bloodUnit.VerificationDate = DateTime.UtcNow;
        }

        if (dto.Status == BloodUnitStatus.Used && !bloodUnit.IssueDate.HasValue)
        {
            bloodUnit.IssueDate = DateTime.UtcNow;
        }

        if (dto.HospitalId.HasValue)
        {
            bloodUnit.HospitalId = dto.HospitalId.Value;
        }

        await _context.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<IEnumerable<BloodUnitResponseDto>> GetAllAsync()
    {
        var units = await _context.BloodUnits
            .Include(u => u.Hospital)
            .Include(u => u.Verifier)
            .Include(u => u.Donor)
            .Include(u => u.Registration)
                .ThenInclude(r => r.User)
            .ToListAsync();

        return units.Select(BloodUnitResponseDto.FromEntity);
    }

    public async Task<BloodUnitResponseDto> GetByIdAsync(int id)
    {
        var unit = await FindUnitEntityByIdAsync(id);
        return BloodUnitResponseDto.FromEntity(unit);
    }

    // --- Private Helper Methods ---

    private async Task<BloodUnit> FindUnitEntityByIdAsync(int id)
    {
        var unit = await _context.BloodUnits
            .Include(u => u.Hospital)
            .Include(u => u.Verifier)
            .Include(u => u.Donor)
            .Include(u => u.Registration)
                .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (unit == null)
        {
            throw new KeyNotFoundException($"Không tìm thấy đơn vị máu với ID: {id}");
        }
        return unit;
    }

    private DateTime CalculateExpiryDate(DateTime donationDate, BloodProductType productType)
    {
        switch (productType)
        {
            case BloodProductType.WholeBlood:
            case BloodProductType.RedCells:
                return donationDate.AddDays(35);

            case BloodProductType.Platelets:
                return donationDate.AddDays(5); 

            case BloodProductType.Plasma:
                return donationDate.AddYears(1);

            default:
                return donationDate.AddDays(35);
        }
    }
}
