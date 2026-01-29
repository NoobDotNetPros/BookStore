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
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            </head>
            <body style='margin: 0; padding: 0; font-family: Arial, Helvetica, sans-serif; background-color: #f4f4f4;'>
                <table role='presentation' style='width: 100%; border-collapse: collapse;'>
                    <tr>
                        <td align='center' style='padding: 40px 0;'>
                            <table role='presentation' style='width: 600px; border-collapse: collapse; background-color: #ffffff; border-radius: 8px; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);'>
                                <!-- Header -->
                                <tr>
                                    <td style='background: linear-gradient(135deg, #a33a3a 0%, #7a2a2a 100%); padding: 30px 40px; border-radius: 8px 8px 0 0; text-align: center;'>
                                        <table role='presentation' style='width: 100%;'>
                                            <tr>
                                                <td align='center'>
                                                    <div style='background-color: rgba(255,255,255,0.15); display: inline-block; padding: 12px 20px; border-radius: 8px;'>
                                                        <span style='font-size: 28px; color: #ffffff; font-weight: bold; letter-spacing: 1px;'>📚 Bookstore</span>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                
                                <!-- Welcome Icon -->
                                <tr>
                                    <td align='center' style='padding: 40px 40px 20px 40px;'>
                                        <div style='background-color: #f0f7f0; width: 80px; height: 80px; border-radius: 50%; display: inline-block; text-align: center; line-height: 80px;'>
                                            <span style='font-size: 40px;'>✉️</span>
                                        </div>
                                    </td>
                                </tr>
                                
                                <!-- Main Content -->
                                <tr>
                                    <td style='padding: 0 40px 30px 40px; text-align: center;'>
                                        <h1 style='color: #333333; font-size: 24px; margin: 0 0 10px 0; font-weight: 600;'>Welcome to Bookstore!</h1>
                                        <p style='color: #666666; font-size: 16px; line-height: 24px; margin: 0;'>Thanks for signing up. Please verify your email address to get started.</p>
                                    </td>
                                </tr>
                                
                                <!-- Verify Button -->
                                <tr>
                                    <td align='center' style='padding: 0 40px 30px 40px;'>
                                        <a href='{verificationLink}' style='display: inline-block; background: linear-gradient(135deg, #a33a3a 0%, #c44d4d 100%); color: #ffffff; text-decoration: none; padding: 16px 40px; border-radius: 50px; font-size: 16px; font-weight: bold; letter-spacing: 0.5px; box-shadow: 0 4px 15px rgba(163, 58, 58, 0.3);'>
                                            ✓ Verify Email Address
                                        </a>
                                    </td>
                                </tr>
                                
                                <!-- Divider -->
                                <tr>
                                    <td style='padding: 0 40px;'>
                                        <table role='presentation' style='width: 100%; border-collapse: collapse;'>
                                            <tr>
                                                <td style='border-bottom: 1px solid #eeeeee;'></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                
                                <!-- Alternative Link -->
                                <tr>
                                    <td style='padding: 30px 40px; text-align: center;'>
                                        <p style='color: #888888; font-size: 14px; margin: 0 0 15px 0;'>Or copy and paste this link in your browser:</p>
                                        <div style='background-color: #f8f9fa; padding: 15px; border-radius: 6px; border: 1px solid #e9ecef; word-break: break-all;'>
                                            <a href='{verificationLink}' style='color: #a33a3a; font-size: 13px; text-decoration: none;'>{verificationLink}</a>
                                        </div>
                                    </td>
                                </tr>
                                
                                <!-- Expiry Notice -->
                                <tr>
                                    <td align='center' style='padding: 0 40px 30px 40px;'>
                                        <div style='background-color: #fff8e6; border-left: 4px solid #ffc107; padding: 15px 20px; border-radius: 0 6px 6px 0; text-align: left;'>
                                            <p style='color: #856404; font-size: 14px; margin: 0;'>
                                                <strong>⏰ Note:</strong> This verification link will expire in <strong>24 hours</strong>.
                                            </p>
                                        </div>
                                    </td>
                                </tr>
                                
                                <!-- Footer -->
                                <tr>
                                    <td style='background-color: #f8f9fa; padding: 30px 40px; border-radius: 0 0 8px 8px; text-align: center;'>
                                        <p style='color: #888888; font-size: 13px; margin: 0 0 10px 0;'>If you didn't create an account, you can safely ignore this email.</p>
                                        <p style='color: #aaaaaa; font-size: 12px; margin: 0;'>© 2026 Bookstore App. All rights reserved.</p>
                                    </td>
                                </tr>
                            </table>
                            
                            <!-- Additional Footer Info -->
                            <table role='presentation' style='width: 600px; margin-top: 20px;'>
                                <tr>
                                    <td align='center'>
                                        <p style='color: #999999; font-size: 11px; margin: 0;'>
                                            This email was sent to <span style='color: #666666;'>{email}</span>
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
