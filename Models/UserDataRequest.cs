namespace HealthyBites.Api.Models
{
    public class UserDataRequest
    {
        public string Gender { get; set; } = string.Empty; // "Men" أو "Women"
        public double WeightKg { get; set; }
        public double HeightCm { get; set; }
        public int AgeYears { get; set; }
        public string ActivityLevel { get; set; } = string.Empty; // "sedentary", "lightly active", "moderately active", "very active", "extra active"
        public string Goal { get; set; } = string.Empty; // "lose", "maintain", "gain"
        public string GoalSpeed { get; set; } = string.Empty; // "moderate", "slow", "fast", "lean"
    }
}