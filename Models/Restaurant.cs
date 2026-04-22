namespace HealthyBites.Api.Models
{
    public class Restaurant
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
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserRestaurant> UserRestaurants { get; set; } = new List<UserRestaurant>();
        public ICollection<RestaurantReview> Reviews { get; set; } = new List<RestaurantReview>();
    }
}
