namespace Bookstore.Business.Interfaces;

public interface IEmailService
{
    Task SendVerificationEmailAsync(string email, string token, CancellationToken cancellationToken = default);
    Task SendPasswordResetOtpAsync(string email, string otp, CancellationToken cancellationToken = default);
}
