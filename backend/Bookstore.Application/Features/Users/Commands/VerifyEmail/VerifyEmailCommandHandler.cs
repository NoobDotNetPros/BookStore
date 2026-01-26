using Bookstore.Application.Contracts.Repositories;
using MediatR;

namespace Bookstore.Application.Features.Users.Commands.VerifyEmail;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyEmailCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByVerificationTokenAsync(request.Token, cancellationToken);

        if (user == null)
            return false;

        if (user.VerificationTokenExpiry.HasValue && user.VerificationTokenExpiry.Value < DateTime.UtcNow)
            return false;

        user.IsEmailVerified = true;
        user.VerificationToken = null;
        user.VerificationTokenExpiry = null;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
