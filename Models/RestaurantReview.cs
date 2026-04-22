namespace HealthyBites.Api.Models
{
    public class RestaurantReview
    {
        public Guid Id { get; set; }
        public Guid RestaurantId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Restaurant Restaurant { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
