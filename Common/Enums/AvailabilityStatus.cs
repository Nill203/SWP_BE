namespace BloodDonationBE.Common.Enums;

/// <summary>
/// Trạng thái sẵn sàng hiến máu của người dùng.
/// </summary>
public enum AvailabilityStatus
{
    /// <summary>
    /// Sẵn sàng để hiến máu.
    /// </summary>
    Available,

    /// <summary>
    /// Đang trong thời gian phục hồi, chưa thể hiến.
    /// </summary>
    Recovering,

    /// <summary>
    /// Tạm thời không sẵn sàng vì lý do cá nhân.
    /// </summary>
    NotAvailable
}
