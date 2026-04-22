using HealthyBites.Api.Models;

namespace HealthyBites.Api.Services
{
    public interface ITokenService
    {
        string CreateAccessToken(User user);
    }
}
