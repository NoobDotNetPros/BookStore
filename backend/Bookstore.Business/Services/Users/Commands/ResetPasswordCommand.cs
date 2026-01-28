using MediatR;

namespace Bookstore.Business.Services.Users.Commands;

public record ResetPasswordCommand(
    string Email,
    string ResetToken,
    string NewPassword,
    string ConfirmPassword
) : IRequest<ResetPasswordResponse>;

public record ResetPasswordResponse(bool Success, string Message);
