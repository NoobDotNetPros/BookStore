using Bookstore.Application.Contracts.Services;

namespace Bookstore.Infrastructure.Services;

public class EmailService : IEmailService
{
    public Task SendVerificationEmailAsync(string email, string token, CancellationToken cancellationToken = default)
    {
        // TODO: Implement actual email sending (SendGrid, SMTP, etc.)
        Console.WriteLine($"Verification email sent to {email} with token: {token}");
        return Task.CompletedTask;
    }
}
