using System.ComponentModel.DataAnnotations;

namespace BloodDonationBE.Features.BloodDonationCampaigns.DTOs;

public class UpdateCampaignDto
{
    [StringLength(255)]
    public string? Name { get; set; }

    [StringLength(255)]
    public string? Address { get; set; }

    public DateTime? ActiveTime { get; set; }

    public string? DonateTime { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Số lượng tối đa phải lớn hơn 0.")]
    public int? Max { get; set; }

    public int? HospitalId { get; set; }

    [Range(-90, 90)]
    public decimal? Lat { get; set; }

    [Range(-180, 180)]
    public decimal? Lng { get; set; }
}
