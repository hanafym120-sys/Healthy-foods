using HealthyBites.Api.Data;
using HealthyBites.Api.DTOs;
using HealthyBites.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthyBites.Api.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly AppDbContext _context;

        public RestaurantService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<RestaurantDto>> GetAllAsync()
        {
            var restaurants = await _context.Restaurants
                .AsNoTracking()
                .Include(x => x.Reviews)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return restaurants.Select(MapRestaurant).ToList();
        }

        public async Task<RestaurantDto> GetByIdAsync(Guid id)
        {
            var restaurant = await _context.Restaurants
                .AsNoTracking()
                .Include(x => x.Reviews)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (restaurant == null)
            {
                throw new InvalidOperationException("Restaurant not found.");
            }

            return MapRestaurant(restaurant);
        }

        public async Task<RestaurantDto> CreateAsync(CreateRestaurantDto request)
        {
            var restaurant = new Restaurant
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Branch = request.Branch?.Trim() ?? string.Empty,
                City = request.City?.Trim() ?? string.Empty,
                Phone = request.Phone.Trim(),
                Address = request.Address.Trim(),
                Category = request.Category?.Trim() ?? string.Empty,
                HasMenuOnline = request.HasMenuOnline,
                GoogleMapsLink = request.GoogleMapsLink?.Trim() ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();

            return MapRestaurant(restaurant);
        }

        public async Task<RestaurantReviewDto> AddReviewAsync(Guid restaurantId, CreateRestaurantReviewDto request)
        {
            var restaurant = await _context.Restaurants
                .Include(x => x.Reviews)
                .FirstOrDefaultAsync(x => x.Id == restaurantId);

            if (restaurant == null)
            {
                throw new InvalidOperationException("Restaurant not found.");
            }

            var userExists = await _context.Users
                .AsNoTracking()
                .AnyAsync(x => x.Id == request.UserId);

            if (!userExists)
            {
                throw new InvalidOperationException("User not found.");
            }

            var review = new RestaurantReview
            {
                Id = Guid.NewGuid(),
                RestaurantId = restaurantId,
                UserId = request.UserId,
                Rating = request.Rating,
                Comment = request.Comment.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _context.RestaurantReviews.Add(review);
            await _context.SaveChangesAsync();

            restaurant.AverageRating = await _context.RestaurantReviews
                .Where(x => x.RestaurantId == restaurantId)
                .AverageAsync(x => (double)x.Rating);

            await _context.SaveChangesAsync();

            return MapReview(review);
        }

        public async Task<IReadOnlyList<RestaurantReviewDto>> GetReviewsAsync(Guid restaurantId)
        {
            var restaurantExists = await _context.Restaurants
                .AsNoTracking()
                .AnyAsync(x => x.Id == restaurantId);

            if (!restaurantExists)
            {
                throw new InvalidOperationException("Restaurant not found.");
            }

            var reviews = await _context.RestaurantReviews
                .AsNoTracking()
                .Where(x => x.RestaurantId == restaurantId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return reviews.Select(MapReview).ToList();
        }

        private static RestaurantDto MapRestaurant(Restaurant restaurant)
        {
            return new RestaurantDto
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Branch = restaurant.Branch,
                City = restaurant.City,
                Phone = restaurant.Phone,
                Address = restaurant.Address,
                Category = restaurant.Category,
                HasMenuOnline = restaurant.HasMenuOnline,
                GoogleMapsLink = restaurant.GoogleMapsLink,
                AverageRating = restaurant.AverageRating,
                CreatedAt = restaurant.CreatedAt,
                Reviews = restaurant.Reviews.Select(MapReview).ToList()
            };
        }

        private static RestaurantReviewDto MapReview(RestaurantReview review)
        {
            return new RestaurantReviewDto
            {
                Id = review.Id,
                RestaurantId = review.RestaurantId,
                UserId = review.UserId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }
    }
}
