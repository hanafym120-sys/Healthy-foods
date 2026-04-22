using System.ComponentModel.DataAnnotations.Schema;

namespace HealthyBites.Api.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public int Age { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public string Gender { get; set; } = null!;
        public string ActivityLevel { get; set; } = null!;
        public string Goal { get; set; } = null!;
        public float DailyCalories { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ResetPasswordOtp { get; set; }
        public DateTime? ResetPasswordOtpExpiresAt { get; set; }
        public string SmokingStatus { get; set; } = "non-smoker";
        public int SleepHours { get; set; } = 7;
        public string StressLevel { get; set; } = "low";
        public bool HasType2Diabetes { get; set; }

        public ICollection<SugarReading> SugarReadings { get; set; } = new List<SugarReading>();
        public ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();
        public ICollection<UserRestaurant> UserRestaurants { get; set; } = new List<UserRestaurant>();
        public ICollection<RestaurantReview> RestaurantReviews { get; set; } = new List<RestaurantReview>();
        public ICollection<DailyCalory> DailyCaloryEntries { get; set; } = new List<DailyCalory>();
        public ICollection<UserDisease> UserDiseases { get; set; } = new List<UserDisease>();

        [NotMapped]
        public IEnumerable<Guid> DiseaseIds => UserDiseases.Select(x => x.DiseaseId);
    }
}
