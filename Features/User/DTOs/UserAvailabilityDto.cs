using BloodDonationBE.Common.Enums;

public class UserAvailabilityDto
{
    public bool IsAvailable { get; set; }
    public string StatusMessage { get; set; } = string.Empty;
    public DateTime? NextAvailableDate { get; set; }
    public BloodProductType? LastDonationType { get; set; }
}