using MediatR;

namespace Bookstore.Business.Services.Users.Commands;

public record RegisterUserCommand(
    string FullName,
    string Email,
    string Password,
    string Phone
) : IRequest<int>;
