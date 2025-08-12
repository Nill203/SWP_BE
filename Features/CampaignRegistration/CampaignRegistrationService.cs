using Microsoft.EntityFrameworkCore;
using BloodDonationBE.Data;
using BloodDonationBE.Features.CampaignRegistrations.DTOs;
using BloodDonationBE.Common.Enums;
using BloodDonationBE.Features.BloodUnits;
using BloodDonationBE.Features.Users;
using BloodDonationBE.Services;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;

namespace BloodDonationBE.Features.CampaignRegistrations;

public class CampaignRegistrationService : ICampaignRegistrationService
{
    private readonly AppDbContext _context;
    private readonly IBloodUnitService _bloodUnitService;
    private readonly IMailService _mailService;

    public CampaignRegistrationService(AppDbContext context, IBloodUnitService bloodUnitService, IMailService mailService)
    {
        _context = context;
        _bloodUnitService = bloodUnitService;
        _mailService = mailService;
    }

    public async Task<RegistrationResponseDto> CreateRegistrationAsync(int userId, CreateRegistrationDto dto)
    {
        var isAlreadyRegistered = await _context.CampaignRegistrations
            .AnyAsync(r => r.UserId == userId && r.CampaignId == dto.CampaignId);

        if (isAlreadyRegistered)
        {
            throw new BadHttpRequestException("Bạn đã đăng ký tham gia sự kiện này rồi.");
        }

        var newRegistration = new CampaignRegistration
        {
            UserId = userId,
            CampaignId = dto.CampaignId,
            Note = dto.Note,
            Status = RegistrationStatus.Confirmed,
            RegisteredAt = DateTime.UtcNow,
            ProductType = dto.ProductType ?? BloodProductType.WholeBlood // <-- THÊM LOGIC NÀY
        };

        await _context.CampaignRegistrations.AddAsync(newRegistration);
        await _context.SaveChangesAsync();

        var created = await _context.CampaignRegistrations
            .Include(r => r.User)
            .Include(r => r.Campaign)
            .FirstAsync(r => r.Id == newRegistration.Id);

        return RegistrationResponseDto.FromEntity(created);
    }

    public async Task<IEnumerable<RegistrationResponseDto>> GetAllRegistrationsAsync()
    {
        var registrations = await _context.CampaignRegistrations
            .Include(r => r.User)
            .Include(r => r.Campaign)
            .ToListAsync();

        return registrations.Select(RegistrationResponseDto.FromEntity);
    }

    public async Task<IEnumerable<RegistrationResponseDto>> GetRegistrationsByCampaignIdAsync(int campaignId)
    {
        var registrations = await _context.CampaignRegistrations
            .Where(r => r.CampaignId == campaignId)
            .Include(r => r.User)
            .Include(r => r.Campaign)
            .ToListAsync();

        return registrations.Select(RegistrationResponseDto.FromEntity);
    }

    public async Task<RegistrationResponseDto> UpdateRegistrationAsync(int registrationId, UpdateRegistrationDto dto)
    {
        var registration = await _context.CampaignRegistrations
            .Include(r => r.User)
            .Include(r => r.Campaign)
            .FirstOrDefaultAsync(r => r.Id == registrationId);

        if (registration == null)
        {
            throw new KeyNotFoundException("Không tìm thấy lượt đăng ký.");
        }

        registration.Status = dto.Status;
        if (dto.Note != null)
        {
            registration.Note = dto.Note;
        }

        if (dto.BloodType.HasValue)
        {
            registration.User.BloodType = dto.BloodType.Value;
            _context.Users.Update(registration.User);
        }

        // Logic khi hiến máu thành công
        if (dto.Status == RegistrationStatus.Completed)
        {
            if (!dto.Volume.HasValue)
            {
                throw new BadHttpRequestException("Lượng máu không được để trống khi trạng thái là COMPLETED.");
            }
            registration.Volume = dto.Volume.Value;

            await _bloodUnitService.CreateFromRegistrationAsync(registration.Id);
            await _context.SaveChangesAsync();

            // Gửi email thông báo
            try
            {
                var user = registration.User;
                var campaign = registration.Campaign;
                var cooldownDays = GetCooldownDays(registration.ProductType);
                var nextDonationDate = campaign.ActiveTime.AddDays(cooldownDays);
                var bloodTypeName = GetBloodTypeName(user.BloodType);
                var subject = $"Cảm ơn {user.FullName} đã hiến máu nhân đạo!";
                var body = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                body {{ font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f4f4f4; }}
                .container {{ width: 100%; max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; border: 1px solid #ddd; }}
                .header {{ background-color: #d9534f; color: #ffffff; padding: 20px; text-align: center; }}
                .header h1 {{ margin: 0; }}
                .content {{ padding: 20px 30px; color: #333; line-height: 1.6; }}
                .info-box {{ background-color: #f9f9f9; border: 1px solid #eee; padding: 15px; border-radius: 5px; margin: 20px 0; }}
                .info-box strong {{ color: #d9534f; }}
                .footer {{ background-color: #f4f4f4; color: #777; padding: 20px; text-align: center; font-size: 12px; }}
                .advice-section h3 {{ color: #d9534f; border-bottom: 2px solid #f0f0f0; padding-bottom: 5px; }}
                .advice-section ul {{ list-style-type: '🩸'; padding-left: 20px; }}
            </style>
        </head>
        <body>
            <div class=""container"">
                <div class=""header"">
                    <h1>Cảm ơn bạn!</h1>
                </div>
                <div class=""content"">
                    <p>Xin chào <strong>{user.FullName}</strong>,</p>
                    <p>Thay mặt ban tổ chức, chúng tôi xin chân thành cảm ơn bạn đã tham gia hiến máu tại sự kiện <strong>{campaign.Name}</strong> vào ngày {campaign.ActiveTime:dd/MM/yyyy}.</p>
                    
                    <div class=""info-box"">
                        <p><strong>Nhóm máu:</strong> {bloodTypeName}</p>
                        <p><strong>Loại máu hiến:</strong> {GetBloodProductTypeName(registration.ProductType)}</p>
                        <p><strong>Lượng máu đã hiến:</strong> {registration.Volume}ml</p>
                        <p><strong>Ngày có thể hiến lại:</strong> Sau ngày {nextDonationDate:dd/MM/yyyy} ({cooldownDays} ngày nghỉ)</p>
                    </div>

                    <div class=""advice-section"">
                        <h3>💡 Lời khuyên chăm sóc sức khỏe sau khi hiến máu</h3>
                        <p>Để đảm bảo sức khỏe hồi phục tốt nhất, bạn vui lòng tham khảo các hướng dẫn sau:</p>
                        <ul>
                            <li><strong>Bổ sung nước:</strong> Uống nhiều nước hoặc nước hoa quả trong 24 giờ đầu để bù lại lượng dịch đã mất.</li>
                            <li><strong>Thực phẩm giàu sắt:</strong> Ăn các món giàu chất sắt như thịt đỏ, gan, trứng, rau có màu xanh đậm (cải bó xôi).</li>
                            <li><strong>Thực phẩm giàu Vitamin C:</strong> Ăn thêm cam, chanh, dâu tây để giúp cơ thể hấp thụ sắt tốt hơn.</li>
                            <li><strong>Nghỉ ngơi hợp lý:</strong> Tránh các hoạt động thể chất gắng sức trong vòng 24 giờ sau khi hiến máu.</li>
                        </ul>
                    </div>

                    <p>Chúc bạn thật nhiều sức khỏe và hy vọng sẽ tiếp tục nhận được sự đồng hành của bạn trong các sự kiện sắp tới!</p>
                    <p>Trân trọng,<br><strong>Đội ngũ BloodDonation</strong></p>
                </div>
                <div class=""footer"">
                    <p>&copy; {DateTime.Now.Year} BloodDonation Project. Mọi quyền được bảo lưu.</p>
                </div>
            </div>
        </body>
        </html>";

                await _mailService.SendMailAsync(user.Email, subject, body);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to send completion email for registration {registration.Id}. Error: {ex.Message}");
            }
        }
        else
        {
            await _context.SaveChangesAsync();
        }

        await _context.Entry(registration).Reference(r => r.Campaign).LoadAsync();
        return RegistrationResponseDto.FromEntity(registration);
    }
    private int GetCooldownDays(BloodProductType type)
    {
        switch (type)
        {
            case BloodProductType.WholeBlood:
            case BloodProductType.RedCells:
                return 12 * 7;
            case BloodProductType.Platelets:
                return 28;
            case BloodProductType.Plasma:
                return 14;
            default:
                return 84;
        }
    }

    private string GetBloodProductTypeName(BloodProductType type)
    {
        return type switch
        {
            BloodProductType.WholeBlood => "máu toàn phần",
            BloodProductType.RedCells => "hồng cầu",
            BloodProductType.Platelets => "tiểu cầu",
            BloodProductType.Plasma => "huyết tương",
            _ => "máu"
        };
    }

    private string GetBloodTypeName(BloodType? bloodType)
    {
        if (!bloodType.HasValue || bloodType.Value == BloodType.None)
        {
            return "Chưa xác định";
        }

        var enumType = typeof(BloodType);
        var memberInfo = enumType.GetMember(bloodType.Value.ToString()).FirstOrDefault();

        if (memberInfo != null)
        {
            var enumMemberAttribute = memberInfo.GetCustomAttribute<EnumMemberAttribute>();
            if (enumMemberAttribute != null)
            {
                return enumMemberAttribute.Value; 
            }
        }

        return bloodType.Value.ToString(); // Fallback nếu không có attribute
    }

    public async Task<IEnumerable<DonationHistoryResponseDto>> GetDonationHistoryByUserIdAsync(int userId)
    {
        return await _context.CampaignRegistrations
            .Where(r => r.UserId == userId && r.Status == RegistrationStatus.Completed && r.Volume.HasValue)
            .Include(r => r.Campaign)
            .OrderByDescending(r => r.Campaign.ActiveTime)
            .Select(r => new DonationHistoryResponseDto
            {
                Date = r.Campaign.ActiveTime.ToString("dd/MM/yyyy"),
                Volume = r.Volume!.Value,
                Location = r.Campaign.Address
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<RegistrationResponseDto>> GetMyRegistrationsAsync(int userId)
    {
        var registrations = await _context.CampaignRegistrations
            .Where(r => r.UserId == userId)
            .Include(r => r.User)
            .Include(r => r.Campaign)
            .OrderByDescending(r => r.Campaign.ActiveTime)
            .ToListAsync();

        return registrations.Select(RegistrationResponseDto.FromEntity);
    }

    public async Task<RegistrationResponseDto> CancelRegistrationAsync(int registrationId, int userId)
    {
        var registration = await _context.CampaignRegistrations
            .Include(r => r.User)
            .Include(r => r.Campaign)
            .FirstOrDefaultAsync(r => r.Id == registrationId);

        if (registration == null)
        {
            throw new KeyNotFoundException("Không tìm thấy lượt đăng ký.");
        }

    
        if (registration.UserId != userId)
        {
            throw new UnauthorizedAccessException("Bạn không có quyền hủy lượt đăng ký này.");
        }      
        if (registration.Status == RegistrationStatus.Completed || registration.Status == RegistrationStatus.Cancelled)
        {
            throw new BadHttpRequestException($"Không thể hủy lượt đăng ký ở trạng thái '{registration.Status}'.");
        }

        registration.Status = RegistrationStatus.Cancelled;
        registration.Note = "Người dùng tự hủy.";

        await _context.SaveChangesAsync();
        return RegistrationResponseDto.FromEntity(registration);
    }
}

