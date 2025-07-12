using BloodDonationBE.Features.BloodUnits.DTOs;

namespace BloodDonationBE.Features.BloodUnits;

public interface IBloodUnitService
{
    /// <summary>
    /// Tự động tạo một đơn vị máu sau khi có lượt hiến máu thành công.
    /// </summary>
    Task CreateFromRegistrationAsync(int registrationId);

    /// <summary>
    /// Tạo một đơn vị máu thủ công (dành cho Admin).
    /// </summary>
    Task<BloodUnitResponseDto> CreateManualAsync(ManualCreateBloodUnitDto dto);

    /// <summary>
    /// Cập nhật trạng thái của một đơn vị máu.
    /// </summary>
    Task<BloodUnitResponseDto> UpdateStatusAsync(int id, UpdateBloodUnitDto dto, int verifierId);

    /// <summary>
    /// Lấy danh sách tất cả các đơn vị máu.
    /// </summary>
    Task<IEnumerable<BloodUnitResponseDto>> GetAllAsync();

    /// <summary>
    /// Lấy thông tin chi tiết của một đơn vị máu theo ID.
    /// </summary>
    Task<BloodUnitResponseDto> GetByIdAsync(int id);
}
