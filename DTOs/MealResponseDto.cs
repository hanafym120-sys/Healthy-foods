using System;

namespace HealthyBites.Api.DTOs
{
    public class MealResponseDto
    {
        public Guid Id { get; set; }
        public string MealName { get; set; } = null!;
        public int Calories { get; set; }
        public DateTime MealDate { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}
