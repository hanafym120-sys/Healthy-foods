namespace HealthyBites.Api.DTOs
{
    public class AuthResultDto
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = null!;
        public UserProfileDto Profile { get; set; } = null!;
    }
}
