using HealthyBites.Api.DTOs;
using HealthyBites.Api.Models;

namespace HealthyBites.Api.Services
{
    public interface IWeightDynamicsService
    {
        WeightDynamicsResponseDto CalculateWeightChange(User user, double dailyIntakeCalories);
    }
}
