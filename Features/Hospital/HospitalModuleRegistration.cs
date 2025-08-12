using BloodDonationBE.Features.Hospitals;

namespace BloodDonationBE.Features.Hospitals;

public static class HospitalModuleRegistration
{
    public static IServiceCollection AddHospitalServices(this IServiceCollection services)
    {
        services.AddScoped<IHospitalService, HospitalService>();
        return services;
    }
}
