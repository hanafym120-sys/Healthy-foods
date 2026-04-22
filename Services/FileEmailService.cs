using System.Text;

namespace HealthyBites.Api.Services
{
    /// <summary>
    /// File-based email service for development and testing.
    /// Saves emails to a local directory instead of sending via SMTP.
    /// </summary>
    public class FileEmailService : IEmailService
    {
        private readonly string _emailsDirectory;
        private readonly ILogger<FileEmailService> _logger;

        public FileEmailService(ILogger<FileEmailService> logger)
        {
            _logger = logger;
            _emailsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "emails_dev");
            
            // Create directory if it doesn't exist
            if (!Directory.Exists(_emailsDirectory))
            {
                Directory.CreateDirectory(_emailsDirectory);
            }
        }

        public async Task SendEmailAsync(string to, string subject, string htmlBody, string? plainTextBody = null)
        {
            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss-fff");
                var sanitizedEmail = to.Replace("@", "_at_").Replace(".", "_");
                var filename = $"{sanitizedEmail}_{timestamp}.txt";
                var filePath = Path.Combine(_emailsDirectory, filename);

                var emailContent = new StringBuilder();
                emailContent.AppendLine("====== EMAIL (DEVELOPMENT MODE) ======");
                emailContent.AppendLine($"To: {to}");
                emailContent.AppendLine($"Subject: {subject}");
                emailContent.AppendLine($"Sent: {DateTime.UtcNow:O}");
                emailContent.AppendLine("======================================");
                emailContent.AppendLine();
                emailContent.AppendLine("--- HTML Body ---");
                emailContent.AppendLine(htmlBody);
                emailContent.AppendLine();
                if (!string.IsNullOrWhiteSpace(plainTextBody))
                {
                    emailContent.AppendLine("--- Plain Text Body ---");
                    emailContent.AppendLine(plainTextBody);
                    emailContent.AppendLine();
                }
                emailContent.AppendLine("======================================");

                await File.WriteAllTextAsync(filePath, emailContent.ToString());

                _logger.LogInformation($"[DEV EMAIL] Saved to: {filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving email to file: {ex.Message}");
                throw;
            }
        }
    }
}
