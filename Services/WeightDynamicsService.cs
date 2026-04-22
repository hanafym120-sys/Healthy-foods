using HealthyBites.Api.DTOs;
using HealthyBites.Api.Models;

namespace HealthyBites.Api.Services
{
    public class WeightDynamicsService : IWeightDynamicsService
    {
        public WeightDynamicsResponseDto CalculateWeightChange(User user, double dailyIntakeCalories)
        {
            var bmr = CalculateBmr(user.Weight, user.Height, user.Age, user.Gender);
            var tdee = bmr * GetActivityFactor(user.ActivityLevel);
            var energyBalance30 = (tdee - dailyIntakeCalories) * 30;
            var theoreticalWeightChange = energyBalance30 / 7700;
            var alphaAdjusted = theoreticalWeightChange * CalculateAlpha(user);
            var finalWeightChange = alphaAdjusted * CalculateBeta(user);

            return new WeightDynamicsResponseDto
            {
                ExpectedWeightChange = Math.Round(finalWeightChange, 2),
                MinChange = Math.Round(finalWeightChange * 0.9, 2),
                MaxChange = Math.Round(finalWeightChange * 1.1, 2)
            };
        }

        private static double CalculateBmr(float weight, float height, int age, string gender)
        {
            return gender.Trim().ToLowerInvariant() == "female"
                ? (10 * weight) + (6.25 * height) - (5 * age) - 161
                : (10 * weight) + (6.25 * height) - (5 * age) + 5;
        }

        private static double GetActivityFactor(string activityLevel)
        {
            return activityLevel.Trim().ToLowerInvariant() switch
            {
                "sedentary" => 1.20,
                "light" => 1.375,
                "moderate" => 1.55,
                "active" => 1.725,
                "very_active" => 1.90,
                _ => 1.20
            };
        }

        private static double CalculateAlpha(User user)
        {
            var bmi = user.Weight / Math.Pow(user.Height / 100.0, 2);

            var alpha = bmi switch
            {
                < 25 => 1.00,
                < 30 => 0.95,
                < 35 => 0.90,
                _ => 0.85
            };

            if (user.ActivityLevel == "sedentary")
            {
                alpha -= 0.05;
            }

            if (user.ActivityLevel == "active" || user.ActivityLevel == "very_active")
            {
                alpha += 0.05;
            }

            if (user.HasType2Diabetes)
            {
                alpha -= 0.05;
            }

            return Math.Clamp(alpha, 0.75, 1.10);
        }

        private static double CalculateBeta(User user)
        {
            var smokingFactor = user.SmokingStatus.Trim().ToLowerInvariant() switch
            {
                "light smoker" => 1.03,
                "heavy smoker" => 1.05,
                "recently quit" => 0.95,
                _ => 1.00
            };

            var sleepFactor = user.SleepHours switch
            {
                >= 7 and <= 8 => 1.00,
                >= 5 and <= 6 => 0.95,
                < 5 => 0.90,
                _ => 0.90
            };

            var stressFactor = user.StressLevel.Trim().ToLowerInvariant() switch
            {
                "moderate" => 0.97,
                "high" => 0.92,
                _ => 1.00
            };

            return smokingFactor * sleepFactor * stressFactor;
        }
    }
}
