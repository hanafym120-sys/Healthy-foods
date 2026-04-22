using System;

namespace HealthyBites.Api.DTOs
{
    public class RecordMealRequestDto
    {
        public Guid UserId { get; set; }
        public string MealName { get; set; } = null!;
        public int Calories { get; set; }
        public DateTime? MealDate { get; set; } = DateTime.UtcNow;
    }
}
