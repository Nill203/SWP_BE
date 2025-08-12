using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BloodDonationBE.Services;

public class MailService : IMailService
{
    private readonly IConfiguration _configuration;

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendMailAsync(string toEmail, string subject, string body)
    {
        var mailSettings = _configuration.GetSection("MailSettings");
        var fromEmail = mailSettings["Mail"];
        var password = mailSettings["Password"];
        var host = mailSettings["Host"];
        var port = int.Parse(mailSettings["Port"]);
        var client = new SmtpClient(host, port)
        {
            EnableSsl = true, // Bật SSL
            Credentials = new NetworkCredential(fromEmail, password)
        };

        // Tạo nội dung email
        var mailMessage = new MailMessage(from: fromEmail, to: toEmail, subject: subject, body: body)
        {
            IsBodyHtml = true 
        };

        // Gửi email
        await client.SendMailAsync(mailMessage);
    }
}
