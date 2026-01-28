using Bookstore.Business.Interfaces;
using MediatR;

namespace Bookstore.Business.Services.Users.Commands;

public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, VerifyOtpResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyOtpCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<VerifyOtpResponse> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        
        if (user == null)
        {
            return new VerifyOtpResponse(false, "Invalid request.");
        }

        // Check if OTP matches
        if (user.PasswordResetOtp != request.Otp)
        {
            return new VerifyOtpResponse(false, "Invalid OTP. Please try again.");
        }

        // Check if OTP has expired
        if (!user.PasswordResetOtpExpiry.HasValue || DateTime.UtcNow > user.PasswordResetOtpExpiry.Value)
        {
            return new VerifyOtpResponse(false, "OTP has expired. Please request a new one.");
        }

        // Generate a reset token for the password reset step
        var resetToken = Guid.NewGuid().ToString();
        
        // Clear OTP and store reset token
        user.PasswordResetOtp = resetToken; // Reuse OTP field to store reset token temporarily
        user.PasswordResetOtpExpiry = DateTime.UtcNow.AddMinutes(15); // Reset token valid for 15 minutes

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new VerifyOtpResponse(true, "OTP verified successfully.", resetToken);
    }
}
