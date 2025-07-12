using BloodDonationBE.Common.Enums;

// ==> Đảm bảo namespace là số nhiều: ...Campaigns.DTOs
namespace BloodDonationBE.Features.BloodDonationCampaigns.DTOs;

public class CampaignResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime ActiveTime { get; set; }
    public string DonateTime { get; set; } = string.Empty;
    public int Max { get; set; }
    public CampaignHospitalDto? Hospital { get; set; }
    public int RegisteredCount { get; set; }
    public CampaignStatus Status { get; set; }
    public decimal? Lat { get; set; }
    public decimal? Lng { get; set; }
}
