using BloodDonationBE.Features.CampaignRegistrations.DTOs;

namespace BloodDonationBE.Features.CampaignRegistrations;

public interface ICampaignRegistrationService
{
    Task<RegistrationResponseDto> CreateRegistrationAsync(int userId, CreateRegistrationDto dto);
    Task<IEnumerable<RegistrationResponseDto>> GetAllRegistrationsAsync();
    Task<IEnumerable<RegistrationResponseDto>> GetRegistrationsByCampaignIdAsync(int campaignId);
    Task<RegistrationResponseDto> UpdateRegistrationAsync(int registrationId, UpdateRegistrationDto dto);
    Task<IEnumerable<DonationHistoryResponseDto>> GetDonationHistoryByUserIdAsync(int userId);
    Task<IEnumerable<RegistrationResponseDto>> GetMyRegistrationsAsync(int userId);
    Task<RegistrationResponseDto> CancelRegistrationAsync(int registrationId, int userId);
}
