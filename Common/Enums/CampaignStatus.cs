namespace BloodDonationBE.Common.Enums;

/// <summary>
/// Trạng thái của một sự kiện hiến máu.
/// </summary>
public enum CampaignStatus
{
    /// <summary>
    /// Sắp diễn ra.
    /// </summary>
    Upcoming,

    /// <summary>
    /// Đang diễn ra.
    /// </summary>
    Ongoing,

    /// <summary>
    /// Đã kết thúc.
    /// </summary>
    Finished,

    /// <summary>
    /// Đã bị hủy.
    /// </summary>
    Cancelled
}
