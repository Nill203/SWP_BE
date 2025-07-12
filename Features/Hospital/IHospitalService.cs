using BloodDonationBE.Features.Hospitals.DTOs;

namespace BloodDonationBE.Features.Hospitals;

public interface IHospitalService
{
    /// <summary>
    /// Tạo một bệnh viện mới.
    /// </summary>
    Task<HospitalResponseDto> CreateHospitalAsync(CreateHospitalDto dto);

    /// <summary>
    /// Lấy danh sách tất cả các bệnh viện.
    /// </summary>
    Task<IEnumerable<HospitalResponseDto>> GetAllHospitalsAsync();

    /// <summary>
    /// Lấy thông tin chi tiết của một bệnh viện theo ID.
    /// </summary>
    Task<HospitalResponseDto> GetHospitalByIdAsync(int id);

    /// <summary>
    /// Cập nhật thông tin của một bệnh viện.
    /// </summary>
    Task<HospitalResponseDto> UpdateHospitalAsync(int id, UpdateHospitalDto dto);

    /// <summary>
    /// Xóa một bệnh viện.
    /// </summary>
    Task DeleteHospitalAsync(int id);
}
