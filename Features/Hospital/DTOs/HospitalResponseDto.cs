using BloodDonationBE.Features.Hospitals;

namespace BloodDonationBE.Features.Hospitals.DTOs;

public class HospitalResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? ContactInfo { get; set; }

    public static HospitalResponseDto FromEntity(Hospital hospital)
    {
        return new HospitalResponseDto
        {
            Id = hospital.Id,
            Name = hospital.Name,
            Address = hospital.Address,
            ContactInfo = hospital.ContactInfo
        };
    }
}
