namespace BloodDonationBE.Common.Enums;

/// <summary>
/// Trạng thái của một yêu cầu cấp phát máu.
/// </summary>
public enum BloodRequestStatus
{
    /// <summary>
    /// Mới tạo, đang chờ nhân viên xử lý.
    /// </summary>
    Pending,

    /// <summary>
    /// Đã được nhân viên xác thực thông tin với bệnh viện.
    /// </summary>
    Verified,

    /// <summary>
    /// Đã bị nhân viên từ chối do thông tin không chính xác.
    /// </summary>
    Rejected,

    /// <summary>
    /// Đã được admin phê duyệt để cấp phát máu từ kho.
    /// </summary>
    Approved,

    /// <summary>
    /// Đã được duyệt nhưng kho không đủ máu, chờ quyết định kêu gọi.
    /// </summary>
    PendingAppeal,

    /// <summary>
    /// Đã hoàn thành việc cấp phát máu.
    /// </summary>
    Fulfilled
}
