using HealthyBites.Api.DTOs;

namespace HealthyBites.Api.Services
{
    public interface IMealService
    {
        Task<IReadOnlyList<MealDto>> GetMealsAsync(string? type, string? search);
        Task<MealDto> GetMealByIdAsync(Guid id);
        Task<IReadOnlyList<MealDto>> GetMealsForUserAsync(Guid userId);
        Task<MealDto> CreateMealAsync(UpsertMealDto request);
        Task<MealDto> UpdateMealAsync(Guid id, UpsertMealDto request);
        Task DeleteMealAsync(Guid id);
    }
}
