using MediatR;

namespace Bookstore.Business.Services.Users.Commands;

public record VerifyEmailCommand(string Token) : IRequest<bool>;
