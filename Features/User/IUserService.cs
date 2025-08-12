using BloodDonationBE.Features.Users.DTOs; 

namespace BloodDonationBE.Features.Users;

public interface IUserService
{
    Task<UserResponseDto> CreateStaffAsync(CreateStaffDto dto);
    Task<UserResponseDto> CreateByAdminAsync(AdminCreateUserDto dto);
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
    Task<UserResponseDto> GetUserByIdAsync(int id);
    Task<UserResponseDto> UpdateUserAsync(int id, UserUpdateDto dto);
    Task DeleteUserAsync(int id);
    Task<string> RegisterAsync(RegisterDto dto);
    Task<string> ConfirmRegistrationAsync(string token);
    Task<LoginResponseDto> LoginAsync(LoginDto dto);
    Task<UserAvailabilityDto> GetUserAvailabilityStatusAsync(int userId);
}
