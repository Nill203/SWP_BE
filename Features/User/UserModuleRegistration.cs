using Microsoft.Extensions.DependencyInjection;
using BloodDonationBE.Services; // Namespace của IMailService và MailService

namespace BloodDonationBE.Features.Users;

public static class UserModuleRegistration
{
    public static IServiceCollection AddUserServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMailService, MailService>();
        services.AddSingleton<PendingUserStore>();

        return services;
    }
}
