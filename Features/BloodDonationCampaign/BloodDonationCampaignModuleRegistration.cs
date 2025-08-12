namespace BloodDonationBE.Features.BloodDonationCampaigns;

public static class BloodDonationCampaignModuleRegistration
{
    public static IServiceCollection AddBloodDonationCampaignServices(this IServiceCollection services)
    {
        services.AddScoped<IBloodDonationCampaignService, BloodDonationCampaignService>();
        return services;
    }
}
