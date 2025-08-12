using BloodDonationBE.Features.BloodUnits;

namespace BloodDonationBE.Features.BloodUnits;

public static class BloodUnitModuleRegistration
{
    public static IServiceCollection AddBloodUnitServices(this IServiceCollection services)
    {
        services.AddScoped<IBloodUnitService, BloodUnitService>();
        return services;
    }
}
