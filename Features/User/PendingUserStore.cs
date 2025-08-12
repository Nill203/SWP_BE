using System.Collections.Concurrent;
using BloodDonationBE.Features.Users.DTOs;

namespace BloodDonationBE.Features.Users;

public class PendingUserStore
{
    private readonly ConcurrentDictionary<string, RegisterDto> _pendingUsers = new();

    public void AddPendingUser(string token, RegisterDto userData)
    {
        _pendingUsers.TryAdd(token, userData);
    }

    public RegisterDto? GetAndRemovePendingUser(string token)
    {
        _pendingUsers.TryRemove(token, out var userData);
        return userData;
    }
}
