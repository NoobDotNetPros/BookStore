using Bookstore.Business.Interfaces;
using MediatR;

namespace Bookstore.Business.Services.Users.Commands;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ForgotPasswordResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public ForgotPasswordCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task<ForgotPasswordResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        
        if (user == null)
        {
            return new ForgotPasswordResponse(false, "No account found with this email address.");
        }

        if (!user.IsEmailVerified)
        {
            return new ForgotPasswordResponse(false, "Please verify your email before resetting your password.");
        }

        // Check if OTP was sent recently (within 3 minutes)
        if (user.LastOtpSentAt.HasValue && 
            DateTime.UtcNow.Subtract(user.LastOtpSentAt.Value).TotalMinutes < 3)
        {
            var remainingSeconds = (int)(180 - DateTime.UtcNow.Subtract(user.LastOtpSentAt.Value).TotalSeconds);
            return new ForgotPasswordResponse(false, $"Please wait {remainingSeconds} seconds before requesting a new OTP.");
        }

        // Generate 6-digit OTP
        var otp = GenerateOtp();
        
        user.PasswordResetOtp = otp;
        user.PasswordResetOtpExpiry = DateTime.UtcNow.AddMinutes(10); // OTP valid for 10 minutes
        user.LastOtpSentAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        try
        {
            await _emailService.SendPasswordResetOtpAsync(user.Email, otp, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send password reset OTP: {ex.Message}");
            return new ForgotPasswordResponse(false, "Failed to send OTP. Please try again later.");
        }

        // Mask the email for privacy
        var maskedEmail = MaskEmail(user.Email);
        return new ForgotPasswordResponse(true, $"OTP has been sent to {maskedEmail}", user.Email);
    }

    private static string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    private static string MaskEmail(string email)
    {
        var parts = email.Split('@');
        if (parts.Length != 2) return email;
        
        var localPart = parts[0];
        var domain = parts[1];
        
        if (localPart.Length <= 2)
            return $"{localPart[0]}***@{domain}";
        
        return $"{localPart[0]}{localPart[1]}***@{domain}";
    }
}
