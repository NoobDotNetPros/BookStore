using MediatR;
using Bookstore.Models.DTOs;

namespace Bookstore.Business.Services.Users.Commands;

public record LoginCommand(string Email, string Password) : IRequest<LoginResponseDto>;
