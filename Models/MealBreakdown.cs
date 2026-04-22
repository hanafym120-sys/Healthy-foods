using System;

namespace HealthyBites.Api.Models
{
    public partial class MealBreakdown
    {
        public Guid Id { get; set; }
        public Guid DailyCaloryId { get; set; }
        public string MealName { get; set; } = null!;
        public int Calories { get; set; }

        public virtual DailyCalory DailyCalory { get; set; } = null!;
    }
}
