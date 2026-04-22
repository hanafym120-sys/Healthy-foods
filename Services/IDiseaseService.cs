using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyBites.Api.DTOs;

namespace HealthyBites.Api.Services
{
    public interface IDiseaseService
    {
        /// <summary>
        /// Get all available diseases in the registry
        /// </summary>
        Task<IEnumerable<DiseaseDto>> GetAllDiseasesAsync();

        /// <summary>
        /// Get active diseases only
        /// </summary>
        Task<IEnumerable<DiseaseDto>> GetActiveDiseasesByCategory(string category);

        /// <summary>
        /// Register a disease for a user
        /// </summary>
        Task<UserDiseaseResponseDto> RegisterUserDiseaseAsync(UserDiseaseRegistrationDto request);

        /// <summary>
        /// Get all diseases registered for a specific user
        /// </summary>
        Task<IEnumerable<UserDiseaseResponseDto>> GetUserDiseasesAsync(Guid userId);

        /// <summary>
        /// Update a user's disease registration details (diagnosed date, notes)
        /// </summary>
        Task<UserDiseaseResponseDto> UpdateUserDiseaseAsync(Guid userDiseaseId, UpdateUserDiseaseRequestDto request);

        /// <summary>
        /// Remove a disease registration from a user
        /// </summary>
        Task<bool> RemoveUserDiseaseAsync(Guid userDiseaseId);

        /// <summary>
        /// Check if a user has a specific disease
        /// </summary>
        Task<bool> UserHasDiseaseAsync(Guid userId, Guid diseaseId);
    }
}
