using MediatR;

namespace Bookstore.Business.Services.Users.Commands;

public record VerifyOtpCommand(string Email, string Otp) : IRequest<VerifyOtpResponse>;

public record VerifyOtpResponse(bool Success, string Message, string? ResetToken = null);
