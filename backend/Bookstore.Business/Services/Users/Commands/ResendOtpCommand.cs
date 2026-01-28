using MediatR;

namespace Bookstore.Business.Services.Users.Commands;

public record ResendOtpCommand(string Email) : IRequest<ResendOtpResponse>;

public record ResendOtpResponse(bool Success, string Message, int? WaitTimeSeconds = null);
