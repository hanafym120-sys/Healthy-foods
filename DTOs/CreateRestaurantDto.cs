namespace HealthyBites.Api.DTOs
{
    public class CreateRestaurantDto
    {
        public string Name { get; set; } = null!;
        public string Branch { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Category { get; set; } = string.Empty;
        public bool HasMenuOnline { get; set; }
        public string GoogleMapsLink { get; set; } = null!;
    }
}
