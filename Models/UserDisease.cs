using System;

namespace HealthyBites.Api.Models
{
    public class UserDisease
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DiseaseId { get; set; }
        public DateTime DiagnosedDate { get; set; }
        public string? Notes { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User User { get; set; } = null!;
        public Disease Disease { get; set; } = null!;
    }
}
