using BloodDonationBE.Features.Users.DTOs; // <-- SỬA DÒNG NÀY

// ==> SỬA NAMESPACE THÀNH SỐ NHIỀU
namespace BloodDonationBE.Features.Users;

/// <summary>
/// Interface định nghĩa các chức năng cho User Service.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Admin tạo tài khoản cho nhân viên hoặc admin khác.
    /// </summary>
    Task<UserResponseDto> CreateStaffAsync(CreateStaffDto dto);

    /// <summary>
    /// Admin tạo tài khoản cho người dùng thông thường.
    /// </summary>
    Task<UserResponseDto> CreateByAdminAsync(AdminCreateUserDto dto);

    /// <summary>
    /// Lấy danh sách tất cả người dùng.
    /// </summary>
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();

    /// <summary>
    /// Tìm một người dùng theo ID.
    /// </summary>
    Task<UserResponseDto> GetUserByIdAsync(int id);

    /// <summary>
    /// Cập nhật thông tin người dùng.
    /// </summary>
    Task<UserResponseDto> UpdateUserAsync(int id, UserUpdateDto dto);

    /// <summary>
    /// Xóa một người dùng.
    /// </summary>
    Task DeleteUserAsync(int id);

    /// <summary>
    /// Người dùng tự đăng ký tài khoản.
    /// </summary>
    /// <returns>Một thông báo hướng dẫn người dùng kiểm tra email.</returns>
    Task<string> RegisterAsync(RegisterDto dto);

    /// <summary>
    /// Xác nhận đăng ký qua token gửi trong email.
    /// </summary>
    /// <returns>Một thông báo xác nhận thành công.</returns>
    Task<string> ConfirmRegistrationAsync(string token);

    /// <summary>
    /// Xử lý đăng nhập và trả về token cùng thông tin người dùng.
    /// </summary>
    Task<LoginResponseDto> LoginAsync(LoginDto dto);
}
