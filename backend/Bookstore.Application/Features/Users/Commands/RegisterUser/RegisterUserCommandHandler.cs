using Bookstore.Application.Contracts.Repositories;
using Bookstore.Application.Contracts.Services;
using Bookstore.Domain.Entities;
using Bookstore.Domain.Enums;
using MediatR;

namespace Bookstore.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, int>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailService _emailService;

    public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IEmailService emailService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _emailService = emailService;
    }

    public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existing = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existing != null)
            throw new InvalidOperationException("User with this email already exists");

        var verificationToken = Guid.NewGuid().ToString();

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            Phone = request.Phone,
            Role = UserRole.User,
            IsEmailVerified = false,
            VerificationToken = verificationToken,
            VerificationTokenExpiry = DateTime.UtcNow.AddHours(24),
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        try
        {
            await _emailService.SendVerificationEmailAsync(user.Email, verificationToken, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send verification email: {ex.Message}");
        }

        return user.Id;
    }
}
