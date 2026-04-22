using System;
using System.Collections.Generic;

namespace HealthyBites.Api.Models
{
    public class DailyCalory
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public int TotalCalories { get; set; }

        public User User { get; set; } = null!;
    }
}
