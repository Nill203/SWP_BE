using BloodDonationBE.Features.BloodDonationCampaigns;

// ==> Đảm bảo namespace là số nhiều để nhất quán
namespace BloodDonationBE.Features.BloodDonationCampaigns;

/// <summary>
/// Lớp tĩnh để đăng ký các dịch vụ liên quan đến module BloodDonationCampaign.
/// </summary>
public static class BloodDonationCampaignModuleRegistration
{
    /// <summary>
    /// Phương thức mở rộng để thêm các dịch vụ của module BloodDonationCampaign vào IServiceCollection.
    /// </summary>
    public static IServiceCollection AddBloodDonationCampaignServices(this IServiceCollection services)
    {
        // Đăng ký Campaign Service: Một instance mới sẽ được tạo cho mỗi request.
        services.AddScoped<IBloodDonationCampaignService, BloodDonationCampaignService>();

        // Thêm các dịch vụ khác của module này nếu có...

        return services;
    }
}
