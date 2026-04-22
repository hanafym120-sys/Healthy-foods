using System;

namespace HealthyBites.Api.DTOs
{
    public class UpdateUserDiseaseRequestDto
    {
        /// <summary>
        /// The date the disease was diagnosed
        /// </summary>
        public DateTime? DiagnosedDate { get; set; }

        /// <summary>
        /// Additional notes about the disease (e.g., severity, treatment plan)
        /// </summary>
        public string? Notes { get; set; }
    }
}
