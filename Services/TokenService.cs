using HealthyBites.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthyBites.Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _key;

        public TokenService(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"] ?? string.Empty;

            if (string.IsNullOrWhiteSpace(_key) || Encoding.UTF8.GetByteCount(_key) < 32)
            {
                throw new ArgumentOutOfRangeException(nameof(configuration), "Jwt:Key must be at least 32 characters.");
            }
        }

        public string CreateAccessToken(User user)
        {
            var claims = new[]
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("email", user.Email)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
