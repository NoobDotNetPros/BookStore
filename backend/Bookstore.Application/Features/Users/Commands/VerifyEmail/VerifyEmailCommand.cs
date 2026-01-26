using MediatR;

namespace Bookstore.Application.Features.Users.Commands.VerifyEmail;

public record VerifyEmailCommand(string Token) : IRequest<bool>;
