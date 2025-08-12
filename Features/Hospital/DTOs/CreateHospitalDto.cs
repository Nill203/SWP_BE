using System.ComponentModel.DataAnnotations;

namespace BloodDonationBE.Features.Hospitals.DTOs;

public class CreateHospitalDto
{
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string Address { get; set; } = string.Empty;

    [StringLength(255)]
    public string? ContactInfo { get; set; }
}
