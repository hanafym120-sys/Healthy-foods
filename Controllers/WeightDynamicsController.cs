using HealthyBites.Api.Data;
using HealthyBites.Api.DTOs;
using HealthyBites.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthyBites.Api.Controllers
{
    [ApiController]
    [Route("api/v1/weight-dynamics")]
    public class WeightDynamicsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWeightDynamicsService _weightDynamicsService;

        public WeightDynamicsController(AppDbContext context, IWeightDynamicsService weightDynamicsService)
        {
            _context = context;
            _weightDynamicsService = weightDynamicsService;
        }

        [HttpPost("estimate")]
        public async Task<IActionResult> Estimate([FromBody] WeightDynamicsRequestDto request)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.UserId);

            if (user == null)
            {
                return NotFound(new { success = false, data = new { }, message = "User not found." });
            }

            var result = _weightDynamicsService.CalculateWeightChange(user, request.DailyIntakeCalories);
            return Ok(new { success = true, data = result, message = "Weight dynamics estimated successfully." });
        }
    }
}
