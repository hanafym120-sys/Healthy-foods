namespace HealthyBites.Api.DTOs
{
    public class RestaurantReviewDto
    {
        public Guid Id { get; set; }
        public Guid RestaurantId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
