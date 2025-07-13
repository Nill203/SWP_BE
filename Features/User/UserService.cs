using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using BloodDonationBE.Features.Users.DTOs;
using BloodDonationBE.Data;
using BloodDonationBE.Common.Enums;
using BloodDonationBE.Services;
using System.Security.Cryptography;
using User = BloodDonationBE.Features.Users.User;

namespace BloodDonationBE.Features.Users;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IMailService _mailService;
    private readonly IConfiguration _configuration;
    private readonly PendingUserStore _pendingUserStore;

    public UserService(AppDbContext context, IMailService mailService, IConfiguration configuration, PendingUserStore pendingUserStore)
    {
        _context = context;
        _mailService = mailService;
        _configuration = configuration;
        _pendingUserStore = pendingUserStore;
    }

    public async Task<string> RegisterAsync(RegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
        {
            throw new BadHttpRequestException("Email đã được đăng ký.");
        }

        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        _pendingUserStore.AddPendingUser(token, dto);
        
        var frontendBaseUrl = _configuration["FrontendSettings:BaseUrl"];
        if (string.IsNullOrEmpty(frontendBaseUrl))
        {
            throw new InvalidOperationException("Chưa cấu hình 'FrontendSettings:BaseUrl' trong appsettings.json");
        }
        
        var confirmUrl = $"{frontendBaseUrl}/xac-thuc/{token}";
        var emailBody = $"Vui lòng nhấn vào link sau để xác nhận đăng ký: <a href='{confirmUrl}'>Xác nhận</a>";
        await _mailService.SendMailAsync(dto.Email, "Xác nhận đăng ký tài khoản", emailBody);

        return "Đăng ký thành công. Vui lòng kiểm tra email để xác thực tài khoản.";
    }

    public async Task<string> ConfirmRegistrationAsync(string token)
    {
        var pendingUserData = _pendingUserStore.GetAndRemovePendingUser(token);

        if (pendingUserData == null)
        {
            throw new BadHttpRequestException("Token không hợp lệ hoặc đã hết hạn.");
        }

        if (await _context.Users.AnyAsync(u => u.Email == pendingUserData.Email))
        {
            return "Tài khoản đã được xác nhận trước đó.";
        }

        // --- BẮT ĐẦU PHẦN CẬP NHẬT ---
        var user = new User
        {
            FullName = pendingUserData.FullName,
            Email = pendingUserData.Email,
            PhoneNumber = pendingUserData.PhoneNumber,
            Password = BCrypt.Net.BCrypt.HashPassword(pendingUserData.Password),
            Role = UserRole.Member,
            // Ánh xạ các trường mới từ DTO
            Address = pendingUserData.Address,
            Birthday = pendingUserData.Birthday,
            Gender = pendingUserData.Gender,
            // Mặc định nhóm máu là 'None' và trạng thái sẵn sàng là 'Available'
            BloodType = BloodType.None,
            AvailabilityStatus = AvailabilityStatus.Available,
        };
        // --- KẾT THÚC PHẦN CẬP NHẬT ---

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return "Xác thực tài khoản thành công!";
    }

    // ... các phương thức khác không thay đổi ...
    public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
        {
            throw new BadHttpRequestException("Email hoặc mật khẩu không đúng.");
        }

        var token = GenerateJwtToken(user);

        return new LoginResponseDto
        {
            Token = token,
            User = UserResponseDto.FromEntity(user)
        };
    }

    public async Task<UserResponseDto> CreateByAdminAsync(AdminCreateUserDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.PhoneNumber == dto.PhoneNumber))
        {
            throw new BadHttpRequestException("Số điện thoại đã tồn tại.");
        }
        
        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Password = BCrypt.Net.BCrypt.HashPassword("123456"),
            Role = UserRole.Member,
            Address = dto.Address,
            Birthday = dto.Birthday,
            Gender = dto.Gender,
            BloodType = dto.BloodType ?? BloodType.None,
            Lat = dto.Lat,
            Lng = dto.Lng
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return UserResponseDto.FromEntity(user);
    }

    public async Task<UserResponseDto> CreateStaffAsync(CreateStaffDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email || u.PhoneNumber == dto.PhoneNumber))
        {
            throw new BadHttpRequestException("Email hoặc số điện thoại đã tồn tại.");
        }

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role,
            Address = dto.Address,
            Gender = dto.Gender,
            Birthday = dto.Birthday,
            BloodType = dto.BloodType ?? BloodType.None,
            Lat = dto.Lat,
            Lng = dto.Lng
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        return UserResponseDto.FromEntity(user);
    }
    
    public async Task<UserResponseDto> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("Không tìm thấy người dùng.");
        }
        return UserResponseDto.FromEntity(user);
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _context.Users.ToListAsync();
        return users.Select(user => UserResponseDto.FromEntity(user));
    }

    public async Task<UserResponseDto> UpdateUserAsync(int id, UserUpdateDto dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("Không tìm thấy người dùng.");
        }

        if (!string.IsNullOrEmpty(dto.FullName)) user.FullName = dto.FullName;
        if (!string.IsNullOrEmpty(dto.PhoneNumber)) user.PhoneNumber = dto.PhoneNumber;
        if (!string.IsNullOrEmpty(dto.Address)) user.Address = dto.Address;
        if (!string.IsNullOrEmpty(dto.AvatarImage)) user.AvatarImage = dto.AvatarImage;

        if (dto.Birthday.HasValue) user.Birthday = dto.Birthday.Value;
        if (dto.Gender.HasValue) user.Gender = dto.Gender.Value;
        if (dto.BloodType.HasValue) user.BloodType = dto.BloodType.Value;
        if (dto.Lat.HasValue) user.Lat = dto.Lat.Value;
        if (dto.Lng.HasValue) user.Lng = dto.Lng.Value;

        if (!string.IsNullOrEmpty(dto.Password))
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        }

        await _context.SaveChangesAsync();
        return UserResponseDto.FromEntity(user);
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("Không tìm thấy người dùng.");
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
    
    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
