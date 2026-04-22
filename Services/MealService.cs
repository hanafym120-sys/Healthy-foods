using HealthyBites.Api.Data;
using HealthyBites.Api.DTOs;
using HealthyBites.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthyBites.Api.Services
{
    public class MealService : IMealService
    {
        private readonly AppDbContext _context;

        public MealService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<MealDto>> GetMealsAsync(string? type, string? search)
        {
            var query = _context.Meals
                .AsNoTracking()
                .Where(x => x.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(type))
            {
                var normalizedType = type.Trim().ToLowerInvariant();
                query = query.Where(x => x.MealType.ToLower() == normalizedType);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var normalizedSearch = search.Trim().ToLowerInvariant();
                query = query.Where(x => x.Name.ToLower().Contains(normalizedSearch) || x.Ingredients.ToLower().Contains(normalizedSearch));
            }

            var meals = await query
                .OrderBy(x => x.Name)
                .ToListAsync();

            return meals.Select(MapMeal).ToList();
        }

        public async Task<MealDto> GetMealByIdAsync(Guid id)
        {
            var meal = await _context.Meals
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (meal == null)
            {
                throw new InvalidOperationException("Meal not found.");
            }

            return MapMeal(meal);
        }

        public async Task<IReadOnlyList<MealDto>> GetMealsForUserAsync(Guid userId)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(x => x.UserDiseases)
                .ThenInclude(x => x.Disease)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var diseaseTags = user.UserDiseases
                .Select(x => NormalizeDiseaseTag(x.Disease.Name))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToHashSet();

            var meals = await _context.Meals
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .ToListAsync();

            var filtered = diseaseTags.Count == 0
                ? meals
                : meals.Where(x => MealMatchesAllDiseases(x, diseaseTags)).ToList();

            return filtered.Select(MapMeal).ToList();
        }

        public async Task<MealDto> CreateMealAsync(UpsertMealDto request)
        {
            var meal = new Meal
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                ImageUrl = request.ImageUrl?.Trim() ?? string.Empty,
                Cuisine = request.Cuisine?.Trim() ?? string.Empty,
                ServingSizeG = request.ServingSizeG,
                Ingredients = request.Ingredients?.Trim() ?? string.Empty,
                PreparationMethod = request.PreparationMethod?.Trim() ?? string.Empty,
                MedicalBenefitNote = request.MedicalBenefitNote?.Trim() ?? string.Empty,
                Calories = request.Calories,
                CarbohydratesG = request.CarbohydratesG,
                ProteinG = request.ProteinG,
                TotalFatG = request.TotalFatG,
                SaturatedFatG = request.SaturatedFatG,
                SodiumMg = request.SodiumMg,
                PotassiumMg = request.PotassiumMg,
                MealType = request.MealType?.Trim().ToLowerInvariant() ?? string.Empty,
                SuitableForDiabetesAndHypertension = request.SuitableForDiabetesAndHypertension,
                DiseaseTags = request.DiseaseTags.Select(x => x.Trim().ToLowerInvariant()).Distinct().ToList(),
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Meals.Add(meal);
            await _context.SaveChangesAsync();

            return MapMeal(meal);
        }

        public async Task<MealDto> UpdateMealAsync(Guid id, UpsertMealDto request)
        {
            var meal = await _context.Meals.FirstOrDefaultAsync(x => x.Id == id);

            if (meal == null)
            {
                throw new InvalidOperationException("Meal not found.");
            }

            meal.Name = request.Name.Trim();
            meal.ImageUrl = request.ImageUrl?.Trim() ?? string.Empty;
            meal.Cuisine = request.Cuisine?.Trim() ?? string.Empty;
            meal.ServingSizeG = request.ServingSizeG;
            meal.Ingredients = request.Ingredients?.Trim() ?? string.Empty;
            meal.PreparationMethod = request.PreparationMethod?.Trim() ?? string.Empty;
            meal.MedicalBenefitNote = request.MedicalBenefitNote?.Trim() ?? string.Empty;
            meal.Calories = request.Calories;
            meal.CarbohydratesG = request.CarbohydratesG;
            meal.ProteinG = request.ProteinG;
            meal.TotalFatG = request.TotalFatG;
            meal.SaturatedFatG = request.SaturatedFatG;
            meal.SodiumMg = request.SodiumMg;
            meal.PotassiumMg = request.PotassiumMg;
            meal.MealType = request.MealType?.Trim().ToLowerInvariant() ?? string.Empty;
            meal.SuitableForDiabetesAndHypertension = request.SuitableForDiabetesAndHypertension;
            meal.DiseaseTags = request.DiseaseTags.Select(x => x.Trim().ToLowerInvariant()).Distinct().ToList();
            meal.IsActive = request.IsActive;

            await _context.SaveChangesAsync();
            return MapMeal(meal);
        }

        public async Task DeleteMealAsync(Guid id)
        {
            var meal = await _context.Meals.FirstOrDefaultAsync(x => x.Id == id);

            if (meal == null)
            {
                throw new InvalidOperationException("Meal not found.");
            }

            _context.Meals.Remove(meal);
            await _context.SaveChangesAsync();
        }

        private static MealDto MapMeal(Meal meal)
        {
            return new MealDto
            {
                Id = meal.Id,
                Name = meal.Name,
                ImageUrl = meal.ImageUrl,
                Cuisine = meal.Cuisine,
                ServingSizeG = meal.ServingSizeG,
                Ingredients = meal.Ingredients,
                PreparationMethod = meal.PreparationMethod,
                MedicalBenefitNote = meal.MedicalBenefitNote,
                Calories = meal.Calories,
                CarbohydratesG = meal.CarbohydratesG,
                ProteinG = meal.ProteinG,
                TotalFatG = meal.TotalFatG,
                SaturatedFatG = meal.SaturatedFatG,
                SodiumMg = meal.SodiumMg,
                PotassiumMg = meal.PotassiumMg,
                MealType = meal.MealType,
                SuitableForDiabetesAndHypertension = meal.SuitableForDiabetesAndHypertension,
                DiseaseTags = meal.DiseaseTags,
                IsActive = meal.IsActive,
                CreatedAt = meal.CreatedAt
            };
        }

        private static string NormalizeDiseaseTag(string diseaseName)
        {
            return diseaseName.Trim().ToLowerInvariant().Replace(' ', '_').Replace('-', '_');
        }

        private static bool MealMatchesAllDiseases(Meal meal, HashSet<string> userDiseaseTags)
        {
            var mealDiseaseTags = meal.DiseaseTags
                .Select(tag => tag.Trim().ToLowerInvariant())
                .Where(tag => !string.IsNullOrWhiteSpace(tag))
                .ToHashSet();

            return userDiseaseTags.All(mealDiseaseTags.Contains);
        }
    }
}
