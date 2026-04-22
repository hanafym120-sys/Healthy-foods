using System;

namespace HealthyBites.Api.Models
{
    public class UserRestaurant
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RestaurantId { get; set; }

        public User User { get; set; } = null!;
        public Restaurant Restaurant { get; set; } = null!;
    }
}
