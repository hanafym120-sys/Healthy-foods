using HealthyBites.Api.DTOs;
using HealthyBites.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthyBites.Api.Controllers
{
    [ApiController]
    [Route("api/v1/meals")]
    public class MealController : ControllerBase
    {
        private readonly IMealService _mealService;

        public MealController(IMealService mealService)
        {
            _mealService = mealService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? type, [FromQuery] string? search)
        {
            var result = await _mealService.GetMealsAsync(type, search);
            return Ok(new { success = true, data = result, message = "Meals retrieved successfully." });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _mealService.GetMealByIdAsync(id);
                return Ok(new { success = true, data = result, message = "Meal retrieved successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { success = false, data = new { }, message = ex.Message });
            }
        }

        [HttpGet("for-user/{userId:guid}")]
        public async Task<IActionResult> GetForUser(Guid userId)
        {
            try
            {
                var result = await _mealService.GetMealsForUserAsync(userId);
                return Ok(new { success = true, data = result, message = "User meals retrieved successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { success = false, data = new { }, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UpsertMealDto request)
        {
            var result = await _mealService.CreateMealAsync(request);
            return Ok(new { success = true, data = result, message = "Meal created successfully." });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpsertMealDto request)
        {
            try
            {
                var result = await _mealService.UpdateMealAsync(id, request);
                return Ok(new { success = true, data = result, message = "Meal updated successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { success = false, data = new { }, message = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _mealService.DeleteMealAsync(id);
                return Ok(new { success = true, data = new { }, message = "Meal deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { success = false, data = new { }, message = ex.Message });
            }
        }
    }
}
