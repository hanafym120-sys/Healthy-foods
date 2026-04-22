using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HealthyBites.Api.Data;
using HealthyBites.Api.DTOs;
using HealthyBites.Api.Models;

namespace HealthyBites.Api.Services
{
    public class DiseaseService : IDiseaseService
    {
        private readonly AppDbContext _context;

        public DiseaseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DiseaseDto>> GetAllDiseasesAsync()
        {
            var diseases = await _context.Diseases
                .AsNoTracking()
                .OrderBy(d => d.Category)
                .ThenBy(d => d.Name)
                .Select(d => new DiseaseDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Category = d.Category,
                    IsActive = d.IsActive
                })
                .ToListAsync();

            return diseases;
        }

        public async Task<IEnumerable<DiseaseDto>> GetActiveDiseasesByCategory(string category)
        {
            var diseases = await _context.Diseases
                .AsNoTracking()
                .Where(d => d.IsActive && d.Category == category)
                .OrderBy(d => d.Name)
                .Select(d => new DiseaseDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Category = d.Category,
                    IsActive = d.IsActive
                })
                .ToListAsync();

            return diseases;
        }

        public async Task<UserDiseaseResponseDto> RegisterUserDiseaseAsync(UserDiseaseRegistrationDto request)
        {
            // Validate user exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == request.UserId);
            if (!userExists)
            {
                throw new InvalidOperationException($"User with ID {request.UserId} not found");
            }

            // Validate disease exists
            var disease = await _context.Diseases.FirstOrDefaultAsync(d => d.Id == request.DiseaseId);
            if (disease == null)
            {
                throw new InvalidOperationException($"Disease with ID {request.DiseaseId} not found");
            }

            // Check for duplicate registration
            var existingRegistration = await _context.UserDiseases
                .FirstOrDefaultAsync(ud => ud.UserId == request.UserId && ud.DiseaseId == request.DiseaseId);
            
            if (existingRegistration != null)
            {
                throw new InvalidOperationException("This user already has this disease registered");
            }

            var userDisease = new UserDisease
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                DiseaseId = request.DiseaseId,
                DiagnosedDate = request.DiagnosedDate,
                Notes = request.Notes,
                RegisteredAt = DateTime.UtcNow
            };

            _context.UserDiseases.Add(userDisease);
            await _context.SaveChangesAsync();

            return new UserDiseaseResponseDto
            {
                Id = userDisease.Id,
                UserId = userDisease.UserId,
                Disease = new DiseaseDto
                {
                    Id = disease.Id,
                    Name = disease.Name,
                    Description = disease.Description,
                    Category = disease.Category,
                    IsActive = disease.IsActive
                },
                DiagnosedDate = userDisease.DiagnosedDate,
                Notes = userDisease.Notes,
                RegisteredAt = userDisease.RegisteredAt
            };
        }

        public async Task<IEnumerable<UserDiseaseResponseDto>> GetUserDiseasesAsync(Guid userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                throw new InvalidOperationException($"User with ID {userId} not found");
            }

            var userDiseases = await _context.UserDiseases
                .AsNoTracking()
                .Where(ud => ud.UserId == userId)
                .Include(ud => ud.Disease)
                .OrderByDescending(ud => ud.RegisteredAt)
                .Select(ud => new UserDiseaseResponseDto
                {
                    Id = ud.Id,
                    UserId = ud.UserId,
                    Disease = new DiseaseDto
                    {
                        Id = ud.Disease.Id,
                        Name = ud.Disease.Name,
                        Description = ud.Disease.Description,
                        Category = ud.Disease.Category,
                        IsActive = ud.Disease.IsActive
                    },
                    DiagnosedDate = ud.DiagnosedDate,
                    Notes = ud.Notes,
                    RegisteredAt = ud.RegisteredAt
                })
                .ToListAsync();

            return userDiseases;
        }

        public async Task<UserDiseaseResponseDto> UpdateUserDiseaseAsync(Guid userDiseaseId, UpdateUserDiseaseRequestDto request)
        {
            // Validate input
            if (userDiseaseId == Guid.Empty)
            {
                throw new ArgumentException("User disease ID cannot be empty", nameof(userDiseaseId));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Update request cannot be null");
            }

            // Find the user disease registration
            var userDisease = await _context.UserDiseases
                .Include(ud => ud.Disease)
                .FirstOrDefaultAsync(ud => ud.Id == userDiseaseId);

            if (userDisease == null)
            {
                throw new InvalidOperationException($"User disease registration with ID {userDiseaseId} not found");
            }

            // Update fields if provided
            if (request.DiagnosedDate.HasValue)
            {
                if (request.DiagnosedDate.Value > DateTime.UtcNow)
                {
                    throw new ArgumentException("Diagnosed date cannot be in the future", nameof(request.DiagnosedDate));
                }
                userDisease.DiagnosedDate = request.DiagnosedDate.Value;
            }

            if (!string.IsNullOrWhiteSpace(request.Notes))
            {
                userDisease.Notes = request.Notes.Trim();
            }

            _context.UserDiseases.Update(userDisease);
            await _context.SaveChangesAsync();

            return new UserDiseaseResponseDto
            {
                Id = userDisease.Id,
                UserId = userDisease.UserId,
                Disease = new DiseaseDto
                {
                    Id = userDisease.Disease.Id,
                    Name = userDisease.Disease.Name,
                    Description = userDisease.Disease.Description,
                    Category = userDisease.Disease.Category,
                    IsActive = userDisease.Disease.IsActive
                },
                DiagnosedDate = userDisease.DiagnosedDate,
                Notes = userDisease.Notes,
                RegisteredAt = userDisease.RegisteredAt
            };
        }

        public async Task<bool> RemoveUserDiseaseAsync(Guid userDiseaseId)
        {
            var userDisease = await _context.UserDiseases.FirstOrDefaultAsync(ud => ud.Id == userDiseaseId);
            
            if (userDisease == null)
            {
                throw new InvalidOperationException($"User disease registration with ID {userDiseaseId} not found");
            }

            _context.UserDiseases.Remove(userDisease);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UserHasDiseaseAsync(Guid userId, Guid diseaseId)
        {
            return await _context.UserDiseases
                .AnyAsync(ud => ud.UserId == userId && ud.DiseaseId == diseaseId);
        }
    }
}
