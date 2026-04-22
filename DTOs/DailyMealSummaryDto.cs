using System;
using System.Collections.Generic;

namespace HealthyBites.Api.DTOs
{
    public class DailyMealSummaryDto
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public int TotalCalories { get; set; }
        public int MealCount { get; set; }
        public List<MealResponseDto> Meals { get; set; } = new();
    }
}
