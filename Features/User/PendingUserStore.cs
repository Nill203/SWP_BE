using System.Collections.Concurrent;
using BloodDonationBE.Features.Users.DTOs;

namespace BloodDonationBE.Features.Users;

/// <summary>
/// Lớp lưu trữ trong bộ nhớ (in-memory) cho các tài khoản người dùng đang chờ xác thực.
/// Lớp này được đăng ký dưới dạng Singleton để dữ liệu được duy trì trong suốt vòng đời của ứng dụng.
/// </summary>
public class PendingUserStore
{
    // Sử dụng ConcurrentDictionary để đảm bảo an toàn luồng (thread-safe).
    // Key: Token xác thực (string)
    // Value: Dữ liệu đăng ký của người dùng (RegisterDto)
    private readonly ConcurrentDictionary<string, RegisterDto> _pendingUsers = new();

    /// <summary>
    /// Thêm một người dùng đang chờ xác thực vào kho lưu trữ.
    /// </summary>
    /// <param name="token">Token xác thực duy nhất.</param>
    /// <param name="userData">Dữ liệu đăng ký của người dùng.</param>
    public void AddPendingUser(string token, RegisterDto userData)
    {
        _pendingUsers.TryAdd(token, userData);
    }

    /// <summary>
    /// Lấy và xóa thông tin người dùng đang chờ từ kho lưu trữ.
    /// </summary>
    /// <param name="token">Token xác thực.</param>
    /// <returns>Dữ liệu đăng ký của người dùng nếu token hợp lệ, ngược lại trả về null.</returns>
    public RegisterDto? GetAndRemovePendingUser(string token)
    {
        // Cố gắng lấy và xóa cặp key-value khỏi dictionary.
        _pendingUsers.TryRemove(token, out var userData);
        return userData;
    }
}
