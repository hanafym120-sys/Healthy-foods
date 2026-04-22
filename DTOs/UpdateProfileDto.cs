namespace HealthyBites.Api.DTOs
{
    public class UpdateProfileDto
    {
        public int? Age { get; set; }
        public float? Weight { get; set; }
        public float? Height { get; set; }
        public string? ActivityLevel { get; set; }
        public string? Goal { get; set; }
        public List<Guid>? DiseaseIds { get; set; }
    }
}
