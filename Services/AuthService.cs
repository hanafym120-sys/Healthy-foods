using HealthyBites.Api.Data;
using HealthyBites.Api.DTOs;
using HealthyBites.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace HealthyBites.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public AuthService(AppDbContext context, ITokenService tokenService, IEmailService emailService)
        {
            _context = context;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterDto request)
        {
            await EnsureEmailIsUniqueAsync(request.Email);
            var diseaseIds = await ValidateDiseaseIdsAsync(request.DiseaseIds);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Email = request.Email.Trim().ToLowerInvariant(),
                PasswordHash = HashPassword(request.Password),
                Age = request.Age,
                Weight = request.Weight,
                Height = request.Height,
                Gender = NormalizeGender(request.Gender),
                ActivityLevel = NormalizeActivityLevel(request.ActivityLevel),
                Goal = NormalizeGoal(request.Goal),
                DailyCalories = CalculateDailyCalories(request.Weight, request.Height, request.Age, request.Gender, request.ActivityLevel, request.Goal),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);

            foreach (var diseaseId in diseaseIds)
            {
                _context.UserDiseases.Add(new UserDisease
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    DiseaseId = diseaseId,
                    DiagnosedDate = DateTime.UtcNow,
                    RegisteredAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();

            return new AuthResultDto
            {
                UserId = user.Id,
                Token = _tokenService.CreateAccessToken(user),
                Profile = await BuildUserProfileAsync(user.Id)
            };
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto request)
        {
            var normalizedEmail = request.Email.Trim().ToLowerInvariant();
            var passwordHash = HashPassword(request.Password);

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == normalizedEmail && x.PasswordHash == passwordHash);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid email or password.");
            }

            return new AuthResultDto
            {
                UserId = user.Id,
                Token = _tokenService.CreateAccessToken(user),
                Profile = await BuildUserProfileAsync(user.Id)
            };
        }

        public async Task<string> SendForgotPasswordOtpAsync(ForgotPasswordDto request)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return "If an account with that email exists, an OTP has been sent.";
            }

            user.ResetPasswordOtp = GenerateOtp();
            user.ResetPasswordOtpExpiresAt = DateTime.UtcNow.AddMinutes(10);
            await _context.SaveChangesAsync();

            var subject = "HealthyBites password reset OTP";
            var htmlBody = $"<p>Hello {user.Name},</p><p>Your OTP code is <strong>{user.ResetPasswordOtp}</strong>. It expires in 10 minutes.</p>";
            var plainTextBody = $"Hello {user.Name}, your OTP code is {user.ResetPasswordOtp}. It expires in 10 minutes.";

            await _emailService.SendEmailAsync(user.Email, subject, htmlBody, plainTextBody);
            return "If an account with that email exists, an OTP has been sent.";
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto request)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null ||
                string.IsNullOrWhiteSpace(user.ResetPasswordOtp) ||
                user.ResetPasswordOtpExpiresAt == null ||
                user.ResetPasswordOtpExpiresAt <= DateTime.UtcNow ||
                !string.Equals(user.ResetPasswordOtp, request.OtpCode.Trim(), StringComparison.Ordinal))
            {
                throw new InvalidOperationException("Invalid or expired OTP.");
            }

            user.PasswordHash = HashPassword(request.NewPassword);
            user.ResetPasswordOtp = null;
            user.ResetPasswordOtpExpiresAt = null;

            await _context.SaveChangesAsync();
            return "Password reset successfully.";
        }

        public async Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateProfileDto request)
        {
            var user = await _context.Users
                .Include(x => x.UserDiseases)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            if (request.Age.HasValue)
            {
                user.Age = request.Age.Value;
            }

            if (request.Weight.HasValue)
            {
                user.Weight = request.Weight.Value;
            }

            if (request.Height.HasValue)
            {
                user.Height = request.Height.Value;
            }

            if (!string.IsNullOrWhiteSpace(request.ActivityLevel))
            {
                user.ActivityLevel = NormalizeActivityLevel(request.ActivityLevel);
            }

            if (!string.IsNullOrWhiteSpace(request.Goal))
            {
                user.Goal = NormalizeGoal(request.Goal);
            }

            if (request.DiseaseIds != null)
            {
                var validatedDiseaseIds = await ValidateDiseaseIdsAsync(request.DiseaseIds);
                _context.UserDiseases.RemoveRange(user.UserDiseases);

                foreach (var diseaseId in validatedDiseaseIds)
                {
                    _context.UserDiseases.Add(new UserDisease
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        DiseaseId = diseaseId,
                        DiagnosedDate = DateTime.UtcNow,
                        RegisteredAt = DateTime.UtcNow
                    });
                }
            }

            user.DailyCalories = CalculateDailyCalories(user.Weight, user.Height, user.Age, user.Gender, user.ActivityLevel, user.Goal);

            await _context.SaveChangesAsync();
            return await BuildUserProfileAsync(user.Id);
        }

        public async Task<UserProfileDto> GetProfileAsync(Guid userId)
        {
            return await BuildUserProfileAsync(userId);
        }

        private async Task EnsureEmailIsUniqueAsync(string email)
        {
            var normalizedEmail = email.Trim().ToLowerInvariant();
            var exists = await _context.Users.AnyAsync(x => x.Email == normalizedEmail);
            if (exists)
            {
                throw new InvalidOperationException("Email already exists.");
            }
        }

        private async Task<List<Guid>> ValidateDiseaseIdsAsync(IEnumerable<Guid> diseaseIds)
        {
            var ids = diseaseIds
                .Where(x => x != Guid.Empty)
                .Distinct()
                .ToList();

            if (ids.Count == 0)
            {
                return ids;
            }

            var existingIds = await _context.Diseases
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync();

            if (existingIds.Count != ids.Count)
            {
                throw new InvalidOperationException("One or more diseases are invalid.");
            }

            return ids;
        }

        private async Task<UserProfileDto> BuildUserProfileAsync(Guid userId)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(x => x.UserDiseases)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            return new UserProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Age = user.Age,
                Weight = user.Weight,
                Height = user.Height,
                Gender = user.Gender,
                ActivityLevel = user.ActivityLevel,
                Goal = user.Goal,
                DailyCalories = user.DailyCalories,
                CreatedAt = user.CreatedAt,
                DiseaseIds = user.UserDiseases.Select(x => x.DiseaseId).ToList()
            };
        }

        private static string GenerateOtp()
        {
            return RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(sha.ComputeHash(bytes));
        }

        private static string NormalizeGender(string gender)
        {
            return gender.Trim().ToLowerInvariant() switch
            {
                "male" => "male",
                "female" => "female",
                _ => throw new InvalidOperationException("Gender must be 'male' or 'female'.")
            };
        }

        private static string NormalizeActivityLevel(string activityLevel)
        {
            return activityLevel.Trim().ToLowerInvariant() switch
            {
                "sedentary" => "sedentary",
                "light" => "light",
                "moderate" => "moderate",
                "active" => "active",
                "very_active" => "very_active",
                _ => throw new InvalidOperationException("Invalid activity level.")
            };
        }

        private static string NormalizeGoal(string goal)
        {
            return goal.Trim().ToLowerInvariant() switch
            {
                "loss" => "loss",
                "maintain" => "maintain",
                "gain" => "gain",
                _ => throw new InvalidOperationException("Invalid goal.")
            };
        }

        private static float CalculateDailyCalories(float weight, float height, int age, string gender, string activityLevel, string goal)
        {
            var normalizedGender = NormalizeGender(gender);
            var normalizedActivity = NormalizeActivityLevel(activityLevel);
            var normalizedGoal = NormalizeGoal(goal);

            double bmr = normalizedGender == "male"
                ? (10 * weight) + (6.25 * height) - (5 * age) + 5
                : (10 * weight) + (6.25 * height) - (5 * age) - 161;

            var tdee = bmr * (normalizedActivity switch
            {
                "sedentary" => 1.20,
                "light" => 1.375,
                "moderate" => 1.55,
                "active" => 1.725,
                "very_active" => 1.90,
                _ => 1.20
            });

            var dailyCalories = normalizedGoal switch
            {
                "loss" => tdee - 500,
                "gain" => tdee + 500,
                _ => tdee
            };

            return (float)Math.Round(dailyCalories, 2);
        }
    }
}
