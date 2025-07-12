namespace BloodDonationBE.Common.Enums;

/// <summary>
/// Trạng thái của một đơn vị máu.
/// </summary>
public enum BloodUnitStatus
{
    AwaitingTesting,
    InStock,
    Used,
    Expired,
    Disposed,
    TestingFailed,
}
