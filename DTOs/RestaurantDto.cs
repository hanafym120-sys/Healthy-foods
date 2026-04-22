namespace HealthyBites.Api.DTOs
{
    public class RestaurantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Branch { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Category { get; set; } = string.Empty;
        public bool HasMenuOnline { get; set; }
        public string GoogleMapsLink { get; set; } = null!;
        public double AverageRating { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<RestaurantReviewDto> Reviews { get; set; } = new();
    }
}
