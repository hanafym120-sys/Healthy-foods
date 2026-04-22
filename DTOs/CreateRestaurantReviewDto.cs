namespace HealthyBites.Api.DTOs
{
    public class CreateRestaurantReviewDto
    {
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = null!;
    }
}
