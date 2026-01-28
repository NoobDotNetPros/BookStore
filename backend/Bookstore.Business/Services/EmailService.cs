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
        var verificationLink = $"http://localhost:5000/verify-email?token={token}";
        
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

    public async Task SendPasswordResetOtpAsync(string email, string otp, CancellationToken cancellationToken = default)
    {
        var subject = "Password Reset OTP - Bookstore App";
        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                <div style='background-color: #a33a3a; padding: 20px; text-align: center;'>
                    <h1 style='color: white; margin: 0;'>Bookstore App</h1>
                </div>
                <div style='padding: 30px; background-color: #f9f9f9;'>
                    <h2 style='color: #333;'>Password Reset Request</h2>
                    <p style='color: #555; font-size: 16px;'>You have requested to reset your password. Use the OTP below to proceed:</p>
                    <div style='background-color: #fff; border: 2px dashed #a33a3a; padding: 20px; text-align: center; margin: 20px 0;'>
                        <span style='font-size: 32px; font-weight: bold; letter-spacing: 8px; color: #a33a3a;'>{otp}</span>
                    </div>
                    <p style='color: #555; font-size: 14px;'>This OTP is valid for <strong>10 minutes</strong>.</p>
                    <p style='color: #555; font-size: 14px;'>If you didn't request a password reset, please ignore this email or contact support if you have concerns.</p>
                </div>
                <div style='background-color: #333; padding: 15px; text-align: center;'>
                    <p style='color: #999; font-size: 12px; margin: 0;'>© 2026 Bookstore App. All rights reserved.</p>
                </div>
            </body>
            </html>
        ";

        await SendEmailAsync(email, subject, body, cancellationToken);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken)
    {
        try
        {
            // Validate SMTP settings
            if (string.IsNullOrEmpty(_smtpSettings.Host) || 
                string.IsNullOrEmpty(_smtpSettings.UserName) || 
                string.IsNullOrEmpty(_smtpSettings.Password) ||
                string.IsNullOrEmpty(_smtpSettings.FromEmail))
            {
                throw new InvalidOperationException("SMTP settings are not configured. Please set: Host, UserName, Password, and FromEmail.");
            }

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
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }
}
