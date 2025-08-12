using BloodDonationBE.Features.Hospitals.DTOs;

namespace BloodDonationBE.Features.Hospitals;

public interface IHospitalService
{
    Task<HospitalResponseDto> CreateHospitalAsync(CreateHospitalDto dto);
    Task<IEnumerable<HospitalResponseDto>> GetAllHospitalsAsync();
    Task<HospitalResponseDto> GetHospitalByIdAsync(int id);
    Task<HospitalResponseDto> UpdateHospitalAsync(int id, UpdateHospitalDto dto);
    Task DeleteHospitalAsync(int id);
}
