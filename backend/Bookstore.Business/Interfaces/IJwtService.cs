namespace Bookstore.Business.Interfaces;

public interface IJwtService
{
    string GenerateToken(int userId, string email, string role);
    int? ValidateToken(string token);
}
