using Bookstore.Application.Contracts.Repositories;
using Bookstore.Application.Contracts.Services;
using Bookstore.Application.Features.Users.Dtos;
using MediatR;

namespace Bookstore.Application.Features.Users.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid email or password");

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password");

        if (!user.IsEmailVerified)
            throw new UnauthorizedAccessException("Email not verified. Please check your email for verification link.");

        var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role.ToString());

        return new LoginResponseDto
        {
            Token = token,
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString()
        };
    }
}
