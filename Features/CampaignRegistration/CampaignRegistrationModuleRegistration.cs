using BloodDonationBE.Features.CampaignRegistrations;
using BloodDonationBE.Features.BloodUnits;

namespace BloodDonationBE.Features.CampaignRegistrations;

public static class CampaignRegistrationModuleRegistration
{
    public static IServiceCollection AddCampaignRegistrationServices(this IServiceCollection services)
    {
        services.AddScoped<ICampaignRegistrationService, CampaignRegistrationService>();
        services.AddScoped<IBloodUnitService, BloodUnitService>();

        return services;
    }
}
