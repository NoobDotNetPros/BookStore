using System.Net;
using System.Net.Mail;
using Bookstore.Business.Interfaces;
using Bookstore.Business.Models;
using Microsoft.Extensions.Options;

namespace Bookstore.Business.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendVerificationEmailAsync(string email, string token, CancellationToken cancellationToken = default)
    {
        var verificationLink = $"http://localhost:4200/verify-email?token={token}";
        
        var subject = "Verify Your Email - Bookstore App";
        var body = $@"
            <html>
            <body>
                <h2>Welcome to Bookstore App!</h2>
                <p>Please verify your email address by clicking the link below:</p>
                <p><a href='{verificationLink}'>Verify Email</a></p>
                <p>Or copy and paste this link in your browser:</p>
                <p>{verificationLink}</p>
                <p>This link will expire in 24 hours.</p>
                <br/>
                <p>If you didn't create an account, please ignore this email.</p>
            </body>
            </html>
        ";

        await SendEmailAsync(email, subject, body, cancellationToken);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken)
    {
        try
        {
            using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password),
                EnableSsl = _smtpSettings.EnableSsl
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.FromEmail, _smtpSettings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage, cancellationToken);
        }
        catch (Exception ex)
        {
            // Log the error (add ILogger later)
            Console.WriteLine($"Email sending failed: {ex.Message}");
            throw;
        }
    }
}
