namespace BloodDonationBE.Common.Enums;

/// Trạng thái của một yêu cầu cấp phát máu.
public enum BloodRequestStatus
{
    Pending,
    Verified,
    Rejected,
    Approved,
    PendingAppeal,
    Fulfilled
}
