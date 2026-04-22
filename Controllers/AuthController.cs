using HealthyBites.Api.DTOs;
using HealthyBites.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthyBites.Api.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            try
            {
                var result = await _authService.RegisterAsync(request);
                return Ok(new { success = true, data = result, message = "User registered successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, data = new { }, message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            try
            {
                var result = await _authService.LoginAsync(request);
                return Ok(new { success = true, data = result, message = "Login successful." });
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { success = false, data = new { }, message = ex.Message });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            var message = await _authService.SendForgotPasswordOtpAsync(request);
            return Ok(new { success = true, data = new { }, message });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            try
            {
                var message = await _authService.ResetPasswordAsync(request);
                return Ok(new { success = true, data = new { }, message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, data = new { }, message = ex.Message });
            }
        }

        [HttpPut("profile/{userId:guid}")]
        public async Task<IActionResult> UpdateProfile(Guid userId, [FromBody] UpdateProfileDto request)
        {
            try
            {
                var result = await _authService.UpdateProfileAsync(userId, request);
                return Ok(new { success = true, data = result, message = "Profile updated successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { success = false, data = new { }, message = ex.Message });
            }
        }

        [HttpGet("profile/{userId:guid}")]
        public async Task<IActionResult> GetProfile(Guid userId)
        {
            try
            {
                var result = await _authService.GetProfileAsync(userId);
                return Ok(new { success = true, data = result, message = "Profile retrieved successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { success = false, data = new { }, message = ex.Message });
            }
        }
    }
}
