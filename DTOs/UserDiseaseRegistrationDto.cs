using System;

namespace HealthyBites.Api.DTOs
{
    public class UserDiseaseRegistrationDto
    {
        public Guid UserId { get; set; }
        public Guid DiseaseId { get; set; }
        public DateTime DiagnosedDate { get; set; }
        public string? Notes { get; set; }
    }
}
