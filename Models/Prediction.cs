using System;

namespace HealthyBites.Api.Models
{
    public class Prediction
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SugarReadingId { get; set; }
        public string Result { get; set; } = null!;

        public User User { get; set; } = null!;
        public SugarReading SugarReading { get; set; } = null!;
    }
}
