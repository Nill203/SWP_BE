using BloodDonationBE.Features.BloodDonationCampaigns.DTOs;

// ==> Đảm bảo namespace là số nhiều
namespace BloodDonationBE.Features.BloodDonationCampaigns;

public interface IBloodDonationCampaignService
{
    /// <summary>
    /// Tạo một sự kiện hiến máu mới.
    /// </summary>
    Task<CampaignResponseDto> CreateCampaignAsync(CreateCampaignDto dto);

    /// <summary>
    /// Lấy danh sách tất cả các sự kiện.
    /// </summary>
    Task<IEnumerable<CampaignResponseDto>> GetAllCampaignsAsync();

    /// <summary>
    /// Lấy thông tin chi tiết của một sự kiện theo ID.
    /// </summary>
    Task<CampaignResponseDto> GetCampaignByIdAsync(int id);

    /// <summary>
    /// Cập nhật thông tin của một sự kiện.
    /// </summary>
    Task<CampaignResponseDto> UpdateCampaignAsync(int id, UpdateCampaignDto dto);

    /// <summary>
    /// Xóa một sự kiện.
    /// </summary>
    Task DeleteCampaignAsync(int id);

    /// <summary>
    /// Tìm kiếm các sự kiện trong một khoảng thời gian.
    /// </summary>
    Task<IEnumerable<CampaignResponseDto>> SearchCampaignsByDateRangeAsync(DateTime? start, DateTime? end);
}
