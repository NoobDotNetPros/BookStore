using Bookstore.Business.Interfaces;
using Bookstore.Models.DTOs;
using MediatR;

namespace Bookstore.Business.Services.Users.Commands;

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
        // Hardcoded Admin Check
        if (request.Email == "admin" && request.Password == "admin")
        {
            var adminToken = _jwtService.GenerateToken(0, "admin", "Admin");
            return new LoginResponseDto
            {
                Token = adminToken,
                UserId = 0,
                Email = "admin",
                FullName = "System Admin",
                Role = "Admin"
            };
        }

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
