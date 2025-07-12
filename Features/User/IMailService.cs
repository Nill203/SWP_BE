namespace BloodDonationBE.Services;

/// <summary>
/// Interface định nghĩa dịch vụ gửi email.
/// </summary>
public interface IMailService
{
    /// <summary>
    /// Gửi một email.
    /// </summary>
    /// <param name="toEmail">Email người nhận.</param>
    /// <param name="subject">Chủ đề của email.</param>
    /// <param name="body">Nội dung của email (hỗ trợ HTML).</param>
    Task SendMailAsync(string toEmail, string subject, string body);
}
