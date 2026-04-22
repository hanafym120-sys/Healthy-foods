namespace HealthyBites.Api.DTOs
{
    public class RegisterDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Age { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public string Gender { get; set; } = null!;
        public string ActivityLevel { get; set; } = null!;
        public string Goal { get; set; } = null!;
        public List<Guid> DiseaseIds { get; set; } = new();
    }
}
