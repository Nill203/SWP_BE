using BloodDonationBE.Common.Enums;
using BloodDonationBE.Features.Users;

namespace BloodDonationBE.Features.Users.DTOs;

public class UserResponseDto
{
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public BloodType BloodType { get; set; }
    public UserRole Role { get; set; }
    public string? Address { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public string? AvatarImage { get; set; }
    public decimal? Lat { get; set; }
    public decimal? Lng { get; set; }
    public bool IsAvailable { get; set; }
    public string? AvailabilityStatusMessage { get; set; }
    public DateTime? NextAvailableDonationDate { get; set; }
    public BloodProductType? LastDonationType { get; set; }

    public static UserResponseDto FromEntity(User user, UserAvailabilityDto? availability = null)
    {
        return new UserResponseDto
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            BloodType = user.BloodType,
            Role = user.Role,
            Address = user.Address,
            Gender = user.Gender,
            Birthday = user.Birthday,
            AvatarImage = user.AvatarImage,
            Lat = user.Lat,
            Lng = user.Lng,
            IsAvailable = availability?.IsAvailable ?? true,
            AvailabilityStatusMessage = availability?.StatusMessage ?? "Sẵn sàng hiến máu.",
            NextAvailableDonationDate = availability?.NextAvailableDate,
            LastDonationType = availability?.LastDonationType
        };
    }
}
