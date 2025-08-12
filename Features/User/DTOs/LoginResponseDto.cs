namespace BloodDonationBE.Features.Users.DTOs;

public class LoginResponseDto
{
    public string Token { get; set; }
    public UserResponseDto User { get; set; }
}
