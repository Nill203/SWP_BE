using BloodDonationBE.Features.BloodRequests;

namespace BloodDonationBE.Features.BloodRequests;

/// <summary>
/// Lớp tĩnh để đăng ký các dịch vụ liên quan đến module BloodRequest.
/// </summary>
public static class BloodRequestModuleRegistration
{
    /// <summary>
    /// Phương thức mở rộng để thêm các dịch vụ của module BloodRequest vào IServiceCollection.
    /// </summary>
    public static IServiceCollection AddBloodRequestServices(this IServiceCollection services)
    {
        // Đăng ký BloodRequest Service: Một instance mới sẽ được tạo cho mỗi request.
        services.AddScoped<IBloodRequestService, BloodRequestService>();

        // Thêm các dịch vụ khác của module này nếu có...

        return services;
    }
}
