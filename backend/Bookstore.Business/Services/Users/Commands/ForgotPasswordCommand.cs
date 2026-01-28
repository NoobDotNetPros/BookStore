using MediatR;

namespace Bookstore.Business.Services.Users.Commands;

public record ForgotPasswordCommand(string Email) : IRequest<ForgotPasswordResponse>;

public record ForgotPasswordResponse(bool Success, string Message, string? Email = null);
