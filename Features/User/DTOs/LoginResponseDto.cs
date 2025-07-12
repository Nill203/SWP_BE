namespace BloodDonationBE.Features.Users.DTOs;

/// <summary>
/// Dữ liệu trả về sau khi đăng nhập thành công.
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// JSON Web Token (JWT) để xác thực các yêu cầu sau này.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Thông tin chi tiết của người dùng đã đăng nhập.
    /// </summary>
    public UserResponseDto User { get; set; }
}
