using MediatR;
using Bookstore.Application.Features.Users.Dtos;

namespace Bookstore.Application.Features.Users.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResponseDto>;
