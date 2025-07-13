using BloodDonationBE.Common.Enums;
using BloodDonationBE.Features.CampaignRegistrations;
using BloodDonationBE.Features.Users;

namespace BloodDonationBE.Features.CampaignRegistrations.DTOs;

/// <summary>
/// DTO chứa thông tin tóm tắt của người dùng trong một lượt đăng ký.
/// </summary>
public class RegistrationUserDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public BloodType BloodType { get; set; }
    public UserRole Role { get; set; }
}

/// <summary>
/// DTO chứa thông tin tóm tắt của sự kiện trong một lượt đăng ký.
/// </summary>
public class RegistrationCampaignDto
{
    public int CampaignId { get; set; }
    public string CampaignName { get; set; } = string.Empty;
    public DateTime CampaignDate { get; set; }
    public string Address { get; set; } = string.Empty;
}

/// <summary>
/// DTO chính để trả về thông tin chi tiết của một lượt đăng ký hiến máu.
/// </summary>
public class RegistrationResponseDto
{
    public int Id { get; set; }
    public RegistrationStatus Status { get; set; }
    public BloodProductType ProductType { get; set; } // <-- THÊM TRƯỜNG MỚI
    public string? Note { get; set; }
    public DateTime RegisteredAt { get; set; }
    public int? Volume { get; set; }

    // Thông tin từ các bảng liên quan
    public RegistrationCampaignDto Campaign { get; set; } = null!;
    public RegistrationUserDto User { get; set; } = null!;

    /// <summary>
    /// Phương thức tĩnh để tạo một DTO từ một entity CampaignRegistration.
    /// Yêu cầu entity phải được load kèm theo thông tin User và Campaign.
    /// </summary>
    public static RegistrationResponseDto FromEntity(CampaignRegistration registration)
    {
        return new RegistrationResponseDto
        {
            Id = registration.Id,
            Status = registration.Status,
            ProductType = registration.ProductType, // <-- THÊM ÁNH XẠ
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

/// <summary>
/// DTO để trả về lịch sử hiến máu của một người dùng.
/// </summary>
public class DonationHistoryResponseDto
{
    public string Date { get; set; } = string.Empty;
    public int Volume { get; set; }
    public string Location { get; set; } = string.Empty;
}
