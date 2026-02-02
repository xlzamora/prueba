using TelemedicinaOdonto.Application.DTOs;

namespace TelemedicinaOdonto.Application.Contracts;

public interface IAuthService
{
    Task<AuthResponse> RegisterPatientAsync(RegisterPatientRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<CurrentUserResponse> GetCurrentUserAsync(string userId);
}
