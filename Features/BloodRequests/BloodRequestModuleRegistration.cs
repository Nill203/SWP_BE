using BloodDonationBE.Features.BloodRequests;

namespace BloodDonationBE.Features.BloodRequests;
public static class BloodRequestModuleRegistration
{
    public static IServiceCollection AddBloodRequestServices(this IServiceCollection services)
    {
        services.AddScoped<IBloodRequestService, BloodRequestService>();
        return services;
    }
}
