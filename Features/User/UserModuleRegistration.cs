using Microsoft.Extensions.DependencyInjection;
using BloodDonationBE.Services; // Namespace của IMailService và MailService

namespace BloodDonationBE.Features.Users;

/// <summary>
/// Lớp tĩnh để đăng ký các dịch vụ liên quan đến module User.
/// </summary>
public static class UserModuleRegistration
{
    /// <summary>
    /// Phương thức mở rộng để thêm các dịch vụ của module User vào IServiceCollection.
    /// </summary>
    public static IServiceCollection AddUserServices(this IServiceCollection services)
    {
        // Đăng ký UserService: Một instance mới sẽ được tạo cho mỗi request.
        services.AddScoped<IUserService, UserService>();

        // Đăng ký MailService: Một instance mới sẽ được tạo cho mỗi request.
        services.AddScoped<IMailService, MailService>();

        // Đăng ký PendingUserStore: Chỉ một instance duy nhất được tạo và sử dụng trong suốt vòng đời ứng dụng.
        // Điều này phù hợp để lưu trữ trạng thái trong bộ nhớ (in-memory).
        services.AddSingleton<PendingUserStore>();

        return services;
    }
}
