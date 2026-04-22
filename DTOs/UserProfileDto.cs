namespace HealthyBites.Api.DTOs
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Age { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public string Gender { get; set; } = null!;
        public string ActivityLevel { get; set; } = null!;
        public string Goal { get; set; } = null!;
        public float DailyCalories { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Guid> DiseaseIds { get; set; } = new();
    }
}
