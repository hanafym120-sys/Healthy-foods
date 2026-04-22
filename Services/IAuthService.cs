using HealthyBites.Api.DTOs;

namespace HealthyBites.Api.Services
{
    public interface IAuthService
    {
        Task<AuthResultDto> RegisterAsync(RegisterDto request);
        Task<AuthResultDto> LoginAsync(LoginDto request);
        Task<string> SendForgotPasswordOtpAsync(ForgotPasswordDto request);
        Task<string> ResetPasswordAsync(ResetPasswordDto request);
        Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateProfileDto request);
        Task<UserProfileDto> GetProfileAsync(Guid userId);
    }
}
