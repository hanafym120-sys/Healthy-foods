using System;

namespace HealthyBites.Api.Models
{
    public class SugarReading
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public float Value { get; set; }
        public DateTime Date { get; set; }

        public User User { get; set; } = null!;
        public Prediction? Prediction { get; set; } // optional one-to-one
    }
}
