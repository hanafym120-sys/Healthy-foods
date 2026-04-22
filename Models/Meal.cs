using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace HealthyBites.Api.Models
{
    public class Meal
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = string.Empty;
        public string Cuisine { get; set; } = string.Empty;
        public float? ServingSizeG { get; set; }
        public string Ingredients { get; set; } = string.Empty;
        public string PreparationMethod { get; set; } = string.Empty;
        public string MedicalBenefitNote { get; set; } = string.Empty;
        public float Calories { get; set; }
        public float? CarbohydratesG { get; set; }
        public float? ProteinG { get; set; }
        public float? TotalFatG { get; set; }
        public float? SaturatedFatG { get; set; }
        public float? SodiumMg { get; set; }
        public float? PotassiumMg { get; set; }
        public string MealType { get; set; } = string.Empty;
        public bool SuitableForDiabetesAndHypertension { get; set; }
        public string DiseaseTagsJson { get; set; } = "[]";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public List<string> DiseaseTags
        {
            get => string.IsNullOrWhiteSpace(DiseaseTagsJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(DiseaseTagsJson) ?? new List<string>();
            set => DiseaseTagsJson = JsonSerializer.Serialize(value ?? new List<string>());
        }
    }
}
