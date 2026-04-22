namespace HealthyBites.Api.DTOs
{
    public class WeightDynamicsRequestDto
    {
        public Guid UserId { get; set; }
        public double DailyIntakeCalories { get; set; }
    }
}
