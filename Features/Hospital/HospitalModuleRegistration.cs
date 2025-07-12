using BloodDonationBE.Features.Hospitals;

namespace BloodDonationBE.Features.Hospitals;

/// <summary>
/// Lớp tĩnh để đăng ký các dịch vụ liên quan đến module Hospital.
/// </summary>
public static class HospitalModuleRegistration
{
    /// <summary>
    /// Phương thức mở rộng để thêm các dịch vụ của module Hospital vào IServiceCollection.
    /// </summary>
    public static IServiceCollection AddHospitalServices(this IServiceCollection services)
    {
        // Đăng ký Hospital Service: Một instance mới sẽ được tạo cho mỗi request.
        services.AddScoped<IHospitalService, HospitalService>();

        // Thêm các dịch vụ khác của module này nếu có...

        return services;
    }
}
