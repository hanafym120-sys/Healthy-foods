using HealthyBites.Api.Models;
using System.Text.Json;

namespace HealthyBites.Api.Data
{
    public static class MealSeeder
    {
        public static async Task SeedMealsAsync(AppDbContext dbContext, IWebHostEnvironment environment)
        {
            if (dbContext.Meals.Any())
            {
                return;
            }

            var seedFilePath = Path.Combine(environment.ContentRootPath, "SeedData", "meals.seed.json");
            if (!File.Exists(seedFilePath))
            {
                return;
            }

            var json = await File.ReadAllTextAsync(seedFilePath);
            var seedMeals = JsonSerializer.Deserialize<List<MealSeedItem>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (seedMeals == null || seedMeals.Count == 0)
            {
                return;
            }

            var meals = seedMeals.Select(item => new Meal
            {
                Id = Guid.NewGuid(),
                Name = item.Name,
                ImageUrl = item.ImageUrl ?? string.Empty,
                Cuisine = item.Cuisine ?? string.Empty,
                ServingSizeG = item.ServingSizeG,
                Ingredients = item.Ingredients ?? string.Empty,
                PreparationMethod = item.PreparationMethod ?? string.Empty,
                MedicalBenefitNote = item.MedicalBenefitNote ?? string.Empty,
                Calories = item.Calories,
                CarbohydratesG = item.CarbohydratesG,
                ProteinG = item.ProteinG,
                TotalFatG = item.TotalFatG,
                SaturatedFatG = item.SaturatedFatG,
                SodiumMg = item.SodiumMg,
                PotassiumMg = item.PotassiumMg,
                MealType = item.MealType ?? string.Empty,
                SuitableForDiabetesAndHypertension = item.SuitableForDiabetesAndHypertension,
                DiseaseTags = item.DiseaseTags ?? new List<string>(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            dbContext.Meals.AddRange(meals);
            await dbContext.SaveChangesAsync();
        }

        private sealed class MealSeedItem
        {
            public string Name { get; set; } = string.Empty;
            public string? ImageUrl { get; set; }
            public string? Cuisine { get; set; }
            public float? ServingSizeG { get; set; }
            public string? Ingredients { get; set; }
            public string? PreparationMethod { get; set; }
            public string? MedicalBenefitNote { get; set; }
            public float Calories { get; set; }
            public float? CarbohydratesG { get; set; }
            public float? ProteinG { get; set; }
            public float? TotalFatG { get; set; }
            public float? SaturatedFatG { get; set; }
            public float? SodiumMg { get; set; }
            public float? PotassiumMg { get; set; }
            public string? MealType { get; set; }
            public bool SuitableForDiabetesAndHypertension { get; set; }
            public List<string>? DiseaseTags { get; set; }
        }
    }
}
