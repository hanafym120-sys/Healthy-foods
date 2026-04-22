using System;

namespace HealthyBites.Api.DTOs
{
    public class DiseaseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
