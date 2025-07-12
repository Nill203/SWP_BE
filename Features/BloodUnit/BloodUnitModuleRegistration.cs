using BloodDonationBE.Features.BloodUnits;

namespace BloodDonationBE.Features.BloodUnits;

/// <summary>
/// Lớp tĩnh để đăng ký các dịch vụ liên quan đến module BloodUnit.
/// </summary>
public static class BloodUnitModuleRegistration
{
    /// <summary>
    /// Phương thức mở rộng để thêm các dịch vụ của module BloodUnit vào IServiceCollection.
    /// </summary>
    public static IServiceCollection AddBloodUnitServices(this IServiceCollection services)
    {
        // Đăng ký BloodUnit Service: Một instance mới sẽ được tạo cho mỗi request.
        services.AddScoped<IBloodUnitService, BloodUnitService>();

        // Thêm các dịch vụ khác của module này nếu có...

        return services;
    }
}
