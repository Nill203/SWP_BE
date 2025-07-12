using System.ComponentModel.DataAnnotations;

namespace BloodDonationBE.Features.Hospitals.DTOs;

public class UpdateHospitalDto
{
    [StringLength(255)]
    public string? Name { get; set; }

    [StringLength(255)]
    public string? Address { get; set; }

    [StringLength(255)]
    public string? ContactInfo { get; set; }
}
