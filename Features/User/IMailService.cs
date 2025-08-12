namespace BloodDonationBE.Services;

/// <summary>
/// Interface định nghĩa dịch vụ gửi email.
/// </summary>
public interface IMailService
{
    Task SendMailAsync(string toEmail, string subject, string body);
}
