namespace BloodDonationBE.Common.Enums;

/// <summary>
/// Định nghĩa các trạng thái của một lượt đăng ký hiến máu.
/// </summary>
public enum RegistrationStatus
{
    /// <summary>
    /// Mới đăng ký, đang chờ duyệt.
    /// </summary>
    Pending,

    /// <summary>
    /// Đã xác nhận, người hiến máu dự kiến sẽ đến.
    /// </summary>
    Confirmed,

    /// <summary>
    /// Đã hiến máu thành công.
    /// </summary>
    Completed,
    
    /// <summary>
    /// Đã hủy (bởi người dùng hoặc quản trị viên).
    /// </summary>
    Cancelled,

    /// <summary>
    /// Đã xác nhận nhưng không đến.
    /// </summary>
    Absent,

    /// <summary>
    /// Đến nhưng không đủ điều kiện hiến máu.
    /// </summary>
    NotEligible
}
