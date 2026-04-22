using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyBites.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthyBites.Api.Data
{
    public static class DiseaseSeeder
    {
        public static async Task SeedDiseases(AppDbContext context)
        {
            // Clear existing diseases and reseed with new ones
            // This ensures we have the exact list we want
            var existingDiseases = await context.Diseases.ToListAsync();
            var existingUserDiseases = await context.UserDiseases.ToListAsync();

            // Remove user disease associations first (due to foreign key)
            if (existingUserDiseases.Any())
            {
                context.UserDiseases.RemoveRange(existingUserDiseases);
                await context.SaveChangesAsync();
            }

            // Remove old diseases
            if (existingDiseases.Any())
            {
                context.Diseases.RemoveRange(existingDiseases);
                await context.SaveChangesAsync();
            }

            var diseases = new List<Disease>
            {
                // Type 2 Diabetes
                new Disease
                {
                    Id = Guid.NewGuid(),
                    Name = "Type 2 Diabetes",
                    Description = "A metabolic disorder characterized by high blood sugar levels",
                    Category = "Metabolic",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },

                // Type 1 Diabetes
                new Disease
                {
                    Id = Guid.NewGuid(),
                    Name = "Type 1 Diabetes",
                    Description = "An autoimmune disease where the body doesn't produce insulin",
                    Category = "Metabolic",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },

                // Obesity
                new Disease
                {
                    Id = Guid.NewGuid(),
                    Name = "Obesity",
                    Description = "A condition characterized by excessive body fat accumulation",
                    Category = "Metabolic",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },

                // Underweight
                new Disease
                {
                    Id = Guid.NewGuid(),
                    Name = "Underweight",
                    Description = "A condition characterized by insufficient body weight relative to height",
                    Category = "Metabolic",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },

                // High Blood Pressure (Hypertension)
                new Disease
                {
                    Id = Guid.NewGuid(),
                    Name = "High Blood Pressure",
                    Description = "Persistent elevated blood pressure that may lead to heart disease",
                    Category = "Cardiovascular",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Diseases.AddRangeAsync(diseases);
            await context.SaveChangesAsync();
        }
    }
}
