using BloodDonationBE.Features.CampaignRegistrations.DTOs;

namespace BloodDonationBE.Features.CampaignRegistrations;

public interface ICampaignRegistrationService
{
    /// <summary>
    /// Tạo một lượt đăng ký mới cho người dùng đã đăng nhập.
    /// </summary>
    Task<RegistrationResponseDto> CreateRegistrationAsync(int userId, CreateRegistrationDto dto);

    /// <summary>
    /// Lấy tất cả các lượt đăng ký (dành cho admin/staff).
    /// </summary>
    Task<IEnumerable<RegistrationResponseDto>> GetAllRegistrationsAsync();

    /// <summary>
    /// Lấy tất cả các lượt đăng ký của một sự kiện cụ thể.
    /// </summary>
    Task<IEnumerable<RegistrationResponseDto>> GetRegistrationsByCampaignIdAsync(int campaignId);

    /// <summary>
    /// Cập nhật trạng thái và thông tin của một lượt đăng ký.
    /// </summary>
    Task<RegistrationResponseDto> UpdateRegistrationAsync(int registrationId, UpdateRegistrationDto dto);

    /// <summary>
    /// Lấy lịch sử hiến máu của một người dùng.
    /// </summary>
    Task<IEnumerable<DonationHistoryResponseDto>> GetDonationHistoryByUserIdAsync(int userId);

    /// <summary>
    /// Lấy danh sách các sự kiện mà người dùng hiện tại đã đăng ký.
    /// </summary>
    Task<IEnumerable<RegistrationResponseDto>> GetMyRegistrationsAsync(int userId);

    /// <summary>
    /// Người dùng tự hủy lượt đăng ký của chính mình.
    /// </summary>
    Task<RegistrationResponseDto> CancelRegistrationAsync(int registrationId, int userId);
}
