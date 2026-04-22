using System;
using System.Collections.Generic;

namespace HealthyBites.Api.Models
{
    public class Disease
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = null!; // e.g., "Metabolic", "Cardiovascular", "Endocrine", "Respiratory", etc.
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<UserDisease> UserDiseases { get; set; } = new List<UserDisease>();
    }
}
