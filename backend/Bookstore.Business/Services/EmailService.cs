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
        
        var subject = "Verify your email address";
        var body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Verify Your Email</title>
</head>
<body style=""margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f5f5f5;"">
    <table role=""presentation"" style=""width: 100%; border-collapse: collapse; background-color: #f5f5f5;"">
        <tr>
            <td align=""center"" style=""padding: 40px 0;"">
                <!-- Main Container -->
                <table role=""presentation"" style=""width: 600px; max-width: 90%; border-collapse: collapse; background-color: #ffffff; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);"">
                    
                    <!-- Header -->
                    <tr>
                        <td style=""padding: 48px 40px 32px; text-align: center; border-bottom: 1px solid #f0f0f0;"">
                            <h1 style=""margin: 0; font-size: 24px; font-weight: 600; color: #8F2B2F; line-height: 1.3;"">Welcome to Bookstore</h1>
                        </td>
                    </tr>
                    
                    <!-- Content -->
                    <tr>
                        <td style=""padding: 40px;"">
                            <p style=""margin: 0 0 16px; font-size: 16px; line-height: 1.6; color: #374151;"">
                                Hi there,
                            </p>
                            <p style=""margin: 0 0 24px; font-size: 16px; line-height: 1.6; color: #374151;"">
                                Thank you for signing up! We're excited to have you on board. To get started, please verify your email address by clicking the button below.
                            </p>
                            
                            <!-- CTA Button -->
                            <table role=""presentation"" style=""margin: 32px 0; border-collapse: collapse; width: 100%;"">
                                <tr>
                                    <td align=""center"">
                                        <a href=""{verificationLink}"" style=""display: inline-block; padding: 14px 40px; background: #8F2B2F; color: #ffffff; text-decoration: none; border-radius: 6px; font-size: 16px; font-weight: 600; box-shadow: 0 4px 6px rgba(143, 43, 47, 0.25); transition: transform 0.2s;"">
                                            Verify Email Address
                                        </a>
                                    </td>
                                </tr>
                            </table>
                            
                            <p style=""margin: 24px 0 8px; font-size: 14px; line-height: 1.6; color: #6b7280;"">
                                Or copy and paste this link into your browser:
                            </p>
                            <div style=""padding: 16px; background-color: #f9fafb; border: 1px solid #e5e7eb; border-radius: 6px; word-break: break-all;"">
                                <a href=""{verificationLink}"" style=""color: #8F2B2F; text-decoration: none; font-size: 14px; font-family: 'Courier New', monospace;"">{verificationLink}</a>
                            </div>
                            
                            <!-- Info Box -->
                            <table role=""presentation"" style=""margin: 32px 0; border-collapse: collapse; width: 100%; background-color: #fef3c7; border-left: 4px solid #f59e0b; border-radius: 4px;"">
                                <tr>
                                    <td style=""padding: 16px;"">
                                        <p style=""margin: 0; font-size: 14px; line-height: 1.5; color: #92400e;"">
                                            <strong>⏰ Important:</strong> This verification link will expire in 24 hours for security reasons.
                                        </p>
                                    </td>
                                </tr>
                            </table>
                            
                            <p style=""margin: 24px 0 0; font-size: 14px; line-height: 1.6; color: #6b7280;"">
                                If you didn't create an account with Bookstore, you can safely ignore this email.
                            </p>
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td style=""padding: 32px 40px; background-color: #f9fafb; border-top: 1px solid #e5e7eb; border-radius: 0 0 8px 8px;"">
                            <p style=""margin: 0 0 8px; font-size: 14px; color: #6b7280; text-align: center; line-height: 1.5;"">
                                Thanks for choosing Bookstore!
                            </p>
                            <p style=""margin: 0; font-size: 12px; color: #9ca3af; text-align: center; line-height: 1.5;"">
                                © {DateTime.Now.Year} Bookstore. All rights reserved.
                            </p>
                            <div style=""margin: 16px 0 0; text-align: center;"">
                                <a href=""#"" style=""color: #9ca3af; text-decoration: none; font-size: 12px; margin: 0 8px;"">Privacy Policy</a>
                                <span style=""color: #d1d5db;"">•</span>
                                <a href=""#"" style=""color: #9ca3af; text-decoration: none; font-size: 12px; margin: 0 8px;"">Terms of Service</a>
                                <span style=""color: #d1d5db;"">•</span>
                                <a href=""#"" style=""color: #9ca3af; text-decoration: none; font-size: 12px; margin: 0 8px;"">Contact Us</a>
                            </div>
                        </td>
                    </tr>
                </table>
                
                <!-- Email Client Support Text -->
                <table role=""presentation"" style=""width: 600px; max-width: 90%; margin-top: 16px; border-collapse: collapse;"">
                    <tr>
                        <td style=""text-align: center; padding: 16px;"">
                            <p style=""margin: 0; font-size: 12px; color: #9ca3af; line-height: 1.5;"">
                                This email was sent to <a href=""mailto:{email}"" style=""color: #8F2B2F; text-decoration: none;"">{email}</a>
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
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
