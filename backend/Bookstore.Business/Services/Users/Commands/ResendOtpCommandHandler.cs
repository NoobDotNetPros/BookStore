using Bookstore.Business.Interfaces;
using MediatR;

namespace Bookstore.Business.Services.Users.Commands;

public class ResendOtpCommandHandler : IRequestHandler<ResendOtpCommand, ResendOtpResponse>
{
    private const int OTP_COOLDOWN_SECONDS = 180; // 3 minutes
    
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public ResendOtpCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task<ResendOtpResponse> Handle(ResendOtpCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        
        if (user == null)
        {
            return new ResendOtpResponse(false, "No account found with this email address.");
        }

        if (!user.IsEmailVerified)
        {
            return new ResendOtpResponse(false, "Please verify your email before resetting your password.");
        }

        // Check if OTP was sent recently (within 3 minutes)
        if (user.LastOtpSentAt.HasValue)
        {
            var timeSinceLastOtp = DateTime.UtcNow.Subtract(user.LastOtpSentAt.Value).TotalSeconds;
            if (timeSinceLastOtp < OTP_COOLDOWN_SECONDS)
            {
                var remainingSeconds = (int)(OTP_COOLDOWN_SECONDS - timeSinceLastOtp);
                return new ResendOtpResponse(false, $"Please wait {remainingSeconds} seconds before requesting a new OTP.", remainingSeconds);
            }
        }

        // Generate new 6-digit OTP
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
            Console.WriteLine($"Failed to resend password reset OTP: {ex.Message}");
            return new ResendOtpResponse(false, "Failed to send OTP. Please try again later.");
        }

        return new ResendOtpResponse(true, "A new OTP has been sent to your email.");
    }

    private static string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}
