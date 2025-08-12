using BloodDonationBE.Common.Enums;
using BloodDonationBE.Features.BloodUnits;
using BloodDonationBE.Features.Hospitals;
using BloodDonationBE.Features.Users;

namespace BloodDonationBE.Features.BloodUnits.DTOs;
public class BloodUnitUserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class BloodUnitHospitalDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class BloodUnitResponseDto
{
    public int Id { get; set; }
    public BloodType BloodType { get; set; }
    public int Volume { get; set; }
    public DateTime DonationDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public BloodUnitStatus Status { get; set; }
    public BloodProductType ProductType { get; set; }
    public DateTime? IssueDate { get; set; }
    public BloodUnitHospitalDto? Hospital { get; set; }
    public BloodUnitUserDto? Donor { get; set; }
    public BloodUnitUserDto? Verifier { get; set; }

    public static BloodUnitResponseDto FromEntity(BloodUnit bloodUnit)
    {
        return new BloodUnitResponseDto
        {
            Id = bloodUnit.Id,
            BloodType = bloodUnit.BloodType,
            Volume = bloodUnit.Volume,
            DonationDate = bloodUnit.DonationDate,
            ExpiryDate = bloodUnit.ExpiryDate,
            Status = bloodUnit.Status,
            ProductType = bloodUnit.ProductType,
            IssueDate = bloodUnit.IssueDate,
            Hospital = bloodUnit.Hospital != null 
                ? new BloodUnitHospitalDto { Id = bloodUnit.Hospital.Id, Name = bloodUnit.Hospital.Name } 
                : null,
            Donor = bloodUnit.Donor != null 
                ? new BloodUnitUserDto { Id = bloodUnit.Donor.UserId, Name = bloodUnit.Donor.FullName } 
                : null,
            Verifier = bloodUnit.Verifier != null 
                ? new BloodUnitUserDto { Id = bloodUnit.Verifier.UserId, Name = bloodUnit.Verifier.FullName } 
                : null
        };
    }
}
