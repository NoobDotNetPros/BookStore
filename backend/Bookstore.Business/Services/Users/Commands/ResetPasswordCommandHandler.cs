using Bookstore.Business.Interfaces;
using MediatR;

namespace Bookstore.Business.Services.Users.Commands;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public ResetPasswordCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<ResetPasswordResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        
        if (user == null)
        {
            return new ResetPasswordResponse(false, "Invalid request.");
        }

        // Verify reset token
        if (user.PasswordResetOtp != request.ResetToken)
        {
            return new ResetPasswordResponse(false, "Invalid or expired reset token. Please start the password reset process again.");
        }

        // Check if reset token has expired
        if (!user.PasswordResetOtpExpiry.HasValue || DateTime.UtcNow > user.PasswordResetOtpExpiry.Value)
        {
            return new ResetPasswordResponse(false, "Reset token has expired. Please start the password reset process again.");
        }

        // Verify passwords match
        if (request.NewPassword != request.ConfirmPassword)
        {
            return new ResetPasswordResponse(false, "Passwords do not match.");
        }

        // Update password
        user.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);
        
        // Clear password reset fields
        user.PasswordResetOtp = null;
        user.PasswordResetOtpExpiry = null;
        user.LastOtpSentAt = null;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ResetPasswordResponse(true, "Password has been reset successfully. You can now login with your new password.");
    }
}
