namespace BloodDonationBE.Common.Enums;

public enum UserRole
{
    Member, // Sẽ được lưu là 0 trong DB
    Staff,  // Sẽ được lưu là 1
    Admin   // Sẽ được lưu là 2
}