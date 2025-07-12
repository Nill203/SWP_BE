using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BloodDonationBE.Services;

public class MailService : IMailService
{
    private readonly IConfiguration _configuration;

    // Tiêm IConfiguration để đọc cấu hình từ appsettings.json
    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendMailAsync(string toEmail, string subject, string body)
    {
        // Lấy cấu hình từ appsettings.json
        var mailSettings = _configuration.GetSection("MailSettings");
        var fromEmail = mailSettings["Mail"];
        var password = mailSettings["Password"];
        var host = mailSettings["Host"];
        var port = int.Parse(mailSettings["Port"]);

        // Tạo đối tượng SmtpClient để gửi email qua SMTP của Gmail
        var client = new SmtpClient(host, port)
        {
            EnableSsl = true, // Bật SSL
            Credentials = new NetworkCredential(fromEmail, password)
        };

        // Tạo nội dung email
        var mailMessage = new MailMessage(from: fromEmail, to: toEmail, subject: subject, body: body)
        {
            IsBodyHtml = true // Cho phép nội dung email là HTML để hiển thị link/button đẹp hơn
        };

        // Gửi email
        await client.SendMailAsync(mailMessage);
    }
}
