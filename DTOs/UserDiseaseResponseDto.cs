using System;

namespace HealthyBites.Api.DTOs
{
    public class UserDiseaseResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DiseaseDto Disease { get; set; } = null!;
        public DateTime DiagnosedDate { get; set; }
        public string? Notes { get; set; }
        public DateTime RegisteredAt { get; set; }
    }
}
