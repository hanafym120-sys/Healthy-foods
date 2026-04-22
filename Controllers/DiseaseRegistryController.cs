using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HealthyBites.Api.DTOs;
using HealthyBites.Api.Services;

namespace HealthyBites.Api.Controllers
{
    [ApiController]
    [Route("api/v1/disease-registry")]
    public class DiseaseRegistryController : ControllerBase
    {
        private readonly IDiseaseService _diseaseService;

        public DiseaseRegistryController(IDiseaseService diseaseService)
        {
            _diseaseService = diseaseService;
        }

        /// <summary>
        /// Get all diseases in the registry
        /// </summary>
        /// <returns>List of all available diseases</returns>
        [HttpGet("diseases")]
        public async Task<ActionResult<IEnumerable<DiseaseDto>>> GetAllDiseases()
        {
            try
            {
                var diseases = await _diseaseService.GetAllDiseasesAsync();
                return Ok(new { success = true, data = diseases });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get active diseases by category
        /// </summary>
        /// <param name="category">Disease category (e.g., Metabolic, Cardiovascular, Endocrine, etc.)</param>
        /// <returns>List of diseases in the specified category</returns>
        [HttpGet("diseases/category/{category}")]
        public async Task<ActionResult<IEnumerable<DiseaseDto>>> GetDiseasesByCategory(string category)
        {
            try
            {
                var diseases = await _diseaseService.GetActiveDiseasesByCategory(category);
                return Ok(new { success = true, data = diseases });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Register a disease for a user
        /// </summary>
        /// <param name="request">Disease registration request with user ID, disease ID, and diagnosis date</param>
        /// <returns>Registered disease details</returns>
        [HttpPost("register")]
        public async Task<ActionResult<UserDiseaseResponseDto>> RegisterUserDisease([FromBody] UserDiseaseRegistrationDto request)
        {
            try
            {
                if (request.UserId == Guid.Empty || request.DiseaseId == Guid.Empty)
                {
                    return BadRequest(new { success = false, message = "Invalid user ID or disease ID" });
                }

                var result = await _diseaseService.RegisterUserDiseaseAsync(request);
                return CreatedAtAction(nameof(GetUserDiseases), new { userId = result.UserId }, 
                    new { success = true, data = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get all diseases registered for a user
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>List of user's registered diseases</returns>
        [HttpGet("user/{userId}/diseases")]
        public async Task<ActionResult<IEnumerable<UserDiseaseResponseDto>>> GetUserDiseases(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return BadRequest(new { success = false, message = "Invalid user ID" });
                }

                var diseases = await _diseaseService.GetUserDiseasesAsync(userId);
                return Ok(new { success = true, data = diseases });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Update disease registration details for a user (diagnosis date, notes)
        /// </summary>
        /// <param name="userDiseaseId">The user disease registration ID</param>
        /// <param name="request">Update request with new diagnosis date and/or notes</param>
        /// <returns>Updated disease registration details</returns>
        [HttpPut("{userDiseaseId}")]
        public async Task<ActionResult<UserDiseaseResponseDto>> UpdateUserDisease(Guid userDiseaseId, [FromBody] UpdateUserDiseaseRequestDto request)
        {
            try
            {
                if (userDiseaseId == Guid.Empty)
                {
                    return BadRequest(new { success = false, message = "Invalid user disease ID" });
                }

                if (request == null)
                {
                    return BadRequest(new { success = false, message = "Update request cannot be empty" });
                }

                var result = await _diseaseService.UpdateUserDiseaseAsync(userDiseaseId, request);
                return Ok(new { success = true, data = result, message = "Disease registration updated successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Remove a disease registration from a user
        /// </summary>
        /// <param name="userDiseaseId">The user disease registration ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{userDiseaseId}")]
        public async Task<IActionResult> RemoveUserDisease(Guid userDiseaseId)
        {
            try
            {
                if (userDiseaseId == Guid.Empty)
                {
                    return BadRequest(new { success = false, message = "Invalid user disease ID" });
                }

                await _diseaseService.RemoveUserDiseaseAsync(userDiseaseId);
                return Ok(new { success = true, message = "Disease registration removed successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Check if a user has a specific disease registered
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="diseaseId">The disease ID</param>
        /// <returns>Boolean indicating if user has the disease</returns>
        [HttpGet("user/{userId}/has-disease/{diseaseId}")]
        public async Task<ActionResult<bool>> UserHasDisease(Guid userId, Guid diseaseId)
        {
            try
            {
                if (userId == Guid.Empty || diseaseId == Guid.Empty)
                {
                    return BadRequest(new { success = false, message = "Invalid user ID or disease ID" });
                }

                var hasDisease = await _diseaseService.UserHasDiseaseAsync(userId, diseaseId);
                return Ok(new { success = true, data = hasDisease });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
