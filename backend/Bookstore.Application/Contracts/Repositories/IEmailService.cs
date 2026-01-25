namespace Bookstore.Application.Contracts.Services;

public interface IEmailService
{
    Task SendVerificationEmailAsync(string email, string token, CancellationToken cancellationToken = default);
}
