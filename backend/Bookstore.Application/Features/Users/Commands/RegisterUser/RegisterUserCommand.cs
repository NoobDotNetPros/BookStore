using MediatR;

namespace Bookstore.Application.Features.Users.Commands.RegisterUser;

public record RegisterUserCommand(
    string FullName,
    string Email,
    string Password,
    string Phone
) : IRequest<int>;
