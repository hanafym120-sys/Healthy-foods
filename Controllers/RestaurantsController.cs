using HealthyBites.Api.DTOs;
using HealthyBites.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthyBites.Api.Controllers
{
    [ApiController]
    [Route("api/v1/restaurants")]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _restaurantService.GetAllAsync();
            return Ok(new { success = true, data = result, message = "Restaurants retrieved successfully." });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _restaurantService.GetByIdAsync(id);
                return Ok(new { success = true, data = result, message = "Restaurant retrieved successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { success = false, data = new { }, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRestaurantDto request)
        {
            var result = await _restaurantService.CreateAsync(request);
            return Ok(new { success = true, data = result, message = "Restaurant created successfully." });
        }

        [HttpPost("{id:guid}/reviews")]
        public async Task<IActionResult> AddReview(Guid id, [FromBody] CreateRestaurantReviewDto request)
        {
            try
            {
                var result = await _restaurantService.AddReviewAsync(id, request);
                return Ok(new { success = true, data = result, message = "Review added successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { success = false, data = new { }, message = ex.Message });
            }
        }

        [HttpGet("{id:guid}/reviews")]
        public async Task<IActionResult> GetReviews(Guid id)
        {
            try
            {
                var result = await _restaurantService.GetReviewsAsync(id);
                return Ok(new { success = true, data = result, message = "Reviews retrieved successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { success = false, data = new { }, message = ex.Message });
            }
        }
    }
}
