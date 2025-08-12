using BloodDonationBE.Features.BloodDonationCampaigns.DTOs;
namespace BloodDonationBE.Features.BloodDonationCampaigns;

public interface IBloodDonationCampaignService
{
    /// Tạo một sự kiện hiến máu mới.
    Task<CampaignResponseDto> CreateCampaignAsync(CreateCampaignDto dto);

    /// Lấy danh sách tất cả các sự kiện.
    Task<IEnumerable<CampaignResponseDto>> GetAllCampaignsAsync();

    /// Lấy thông tin chi tiết của một sự kiện theo ID.
    Task<CampaignResponseDto> GetCampaignByIdAsync(int id);

    /// Cập nhật thông tin của một sự kiện.>
    Task<CampaignResponseDto> UpdateCampaignAsync(int id, UpdateCampaignDto dto);

    /// Xóa một sự kiện.
    Task DeleteCampaignAsync(int id);

    /// Tìm kiếm các sự kiện trong một khoảng thời gian.
    Task<IEnumerable<CampaignResponseDto>> SearchCampaignsByDateRangeAsync(DateTime? start, DateTime? end);
}
