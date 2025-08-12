using BloodDonationBE.Common.Enums;
using BloodDonationBE.Features.CampaignRegistrations;
using BloodDonationBE.Features.Users;

namespace BloodDonationBE.Features.CampaignRegistrations.DTOs;

public class RegistrationUserDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public BloodType BloodType { get; set; }
    public UserRole Role { get; set; }
}

public class RegistrationCampaignDto
{
    public int CampaignId { get; set; }
    public string CampaignName { get; set; } = string.Empty;
    public DateTime CampaignDate { get; set; }
    public string Address { get; set; } = string.Empty;
}

public class RegistrationResponseDto
{
    public int Id { get; set; }
    public RegistrationStatus Status { get; set; }
    public BloodProductType ProductType { get; set; }
    public string? Note { get; set; }
    public DateTime RegisteredAt { get; set; }
    public int? Volume { get; set; }

    public RegistrationCampaignDto Campaign { get; set; } = null!;
    public RegistrationUserDto User { get; set; } = null!;

    public static RegistrationResponseDto FromEntity(CampaignRegistration registration)
    {
        return new RegistrationResponseDto
        {
            Id = registration.Id,
            Status = registration.Status,
            ProductType = registration.ProductType, 
            Note = registration.Note,
            RegisteredAt = registration.RegisteredAt,
            Volume = registration.Volume,
            Campaign = new RegistrationCampaignDto
            {
                CampaignId = registration.Campaign.Id,
                CampaignName = registration.Campaign.Name,
                CampaignDate = registration.Campaign.ActiveTime,
                Address = registration.Campaign.Address
            },
            User = new RegistrationUserDto
            {
                UserId = registration.User.UserId,
                UserName = registration.User.FullName,
                Phone = registration.User.PhoneNumber,
                Email = registration.User.Email,
                BloodType = registration.User.BloodType,
                Role = registration.User.Role
            }
        };
    }
}

public class DonationHistoryResponseDto
{
    public string Date { get; set; } = string.Empty;
    public int Volume { get; set; }
    public string Location { get; set; } = string.Empty;
}
