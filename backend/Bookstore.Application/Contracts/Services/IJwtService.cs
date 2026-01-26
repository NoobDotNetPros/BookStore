namespace Bookstore.Application.Contracts.Services;

public interface IJwtService
{
    string GenerateToken(int userId, string email, string role);
    int? ValidateToken(string token);
}
