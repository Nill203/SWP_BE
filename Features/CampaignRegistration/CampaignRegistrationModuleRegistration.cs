using BloodDonationBE.Features.CampaignRegistrations;
using BloodDonationBE.Features.BloodUnits;

namespace BloodDonationBE.Features.CampaignRegistrations;

/// <summary>
/// Lớp tĩnh để đăng ký các dịch vụ liên quan đến module CampaignRegistration.
/// </summary>
public static class CampaignRegistrationModuleRegistration
{
    /// <summary>
    /// Phương thức mở rộng để thêm các dịch vụ của module CampaignRegistration vào IServiceCollection.
    /// </summary>
    public static IServiceCollection AddCampaignRegistrationServices(this IServiceCollection services)
    {
        // Đăng ký Registration Service: Một instance mới sẽ được tạo cho mỗi request.
        services.AddScoped<ICampaignRegistrationService, CampaignRegistrationService>();

        // Vì CampaignRegistrationService phụ thuộc vào IBloodUnitService,
        // chúng ta cũng cần đăng ký nó ở đây (hoặc trong một module riêng).
        // Tạm thời, chúng ta sẽ đăng ký một phiên bản giữ chỗ.
        services.AddScoped<IBloodUnitService, BloodUnitService>();

        return services;
    }
}
