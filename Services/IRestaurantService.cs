using HealthyBites.Api.DTOs;

namespace HealthyBites.Api.Services
{
    public interface IRestaurantService
    {
        Task<IReadOnlyList<RestaurantDto>> GetAllAsync();
        Task<RestaurantDto> GetByIdAsync(Guid id);
        Task<RestaurantDto> CreateAsync(CreateRestaurantDto request);
        Task<RestaurantReviewDto> AddReviewAsync(Guid restaurantId, CreateRestaurantReviewDto request);
        Task<IReadOnlyList<RestaurantReviewDto>> GetReviewsAsync(Guid restaurantId);
    }
}
