using BloodDonationBE.Features.BloodUnits.DTOs;

namespace BloodDonationBE.Features.BloodUnits;

public interface IBloodUnitService
{
    Task CreateFromRegistrationAsync(int registrationId);
    Task<BloodUnitResponseDto> CreateManualAsync(ManualCreateBloodUnitDto dto);
    Task<BloodUnitResponseDto> UpdateStatusAsync(int id, UpdateBloodUnitDto dto, int verifierId);
    Task<IEnumerable<BloodUnitResponseDto>> GetAllAsync();
    Task<BloodUnitResponseDto> GetByIdAsync(int id);
}
