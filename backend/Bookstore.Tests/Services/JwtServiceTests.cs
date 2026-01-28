using Bookstore.Business.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bookstore.Tests.Services;

[TestFixture]
public class JwtServiceTests
{
    private JwtService _jwtService = null!;
    private Mock<IConfiguration> _configurationMock = null!;

    [SetUp]
    public void SetUp()
    {
        _configurationMock = new Mock<IConfiguration>();
        
        // Setup configuration values
        _configurationMock.Setup(c => c["Jwt:Key"]).Returns("ThisIsASecretKeyForTestingPurposesOnly123456!");
        _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("BookstoreTestIssuer");
        _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("BookstoreTestAudience");
        _configurationMock.Setup(c => c["Jwt:ExpiryInMinutes"]).Returns("60");

        _jwtService = new JwtService(_configurationMock.Object);
    }

    [Test]
    public void GenerateToken_ShouldReturnNonEmptyToken()
    {
        // Arrange
        var userId = 1;
        var email = "test@example.com";
        var role = "User";

        // Act
        var token = _jwtService.GenerateToken(userId, email, role);

        // Assert
        Assert.That(string.IsNullOrEmpty(token), Is.False);
    }

    [Test]
    public void GenerateToken_ShouldReturnValidJwtFormat()
    {
        // Arrange
        var userId = 1;
        var email = "test@example.com";
        var role = "User";

        // Act
        var token = _jwtService.GenerateToken(userId, email, role);

        // Assert - JWT has 3 parts separated by dots
        var parts = token.Split('.');
        Assert.That(parts.Length, Is.EqualTo(3));
    }

    [Test]
    public void GenerateToken_ShouldContainCorrectClaims()
    {
        // Arrange
        var userId = 123;
        var email = "test@example.com";
        var role = "Admin";

        // Act
        var token = _jwtService.GenerateToken(userId, email, role);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        Assert.That(jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value, Is.EqualTo(userId.ToString()));
        Assert.That(jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value, Is.EqualTo(email));
        Assert.That(jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value, Is.EqualTo(role));
        Assert.That(jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti), Is.Not.Null);
    }

    [Test]
    public void GenerateToken_ShouldSetCorrectIssuerAndAudience()
    {
        // Arrange
        var userId = 1;
        var email = "test@example.com";
        var role = "User";

        // Act
        var token = _jwtService.GenerateToken(userId, email, role);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        Assert.That(jwtToken.Issuer, Is.EqualTo("BookstoreTestIssuer"));
        Assert.That(jwtToken.Audiences, Does.Contain("BookstoreTestAudience"));
    }

    [Test]
    public void GenerateToken_ShouldSetExpirationTime()
    {
        // Arrange
        var userId = 1;
        var email = "test@example.com";
        var role = "User";

        // Act
        var token = _jwtService.GenerateToken(userId, email, role);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert - Token should expire in approximately 60 minutes
        var expectedExpiry = DateTime.UtcNow.AddMinutes(60);
        Assert.That(jwtToken.ValidTo, Is.GreaterThan(DateTime.UtcNow));
        Assert.That(jwtToken.ValidTo, Is.LessThanOrEqualTo(expectedExpiry.AddSeconds(5))); // Allow small tolerance
    }

    [Test]
    public void ValidateToken_ValidToken_ShouldReturnUserId()
    {
        // Arrange
        var userId = 456;
        var email = "test@example.com";
        var role = "User";
        var token = _jwtService.GenerateToken(userId, email, role);

        // Act
        var result = _jwtService.ValidateToken(token);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(userId));
    }

    [Test]
    public void ValidateToken_InvalidToken_ShouldReturnNull()
    {
        // Arrange
        var invalidToken = "this.is.not.a.valid.token";

        // Act
        var result = _jwtService.ValidateToken(invalidToken);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void ValidateToken_EmptyToken_ShouldReturnNull()
    {
        // Arrange
        var emptyToken = "";

        // Act
        var result = _jwtService.ValidateToken(emptyToken);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void ValidateToken_NullToken_ShouldReturnNull()
    {
        // Act
        var result = _jwtService.ValidateToken(null!);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void ValidateToken_TamperedToken_ShouldReturnNull()
    {
        // Arrange
        var userId = 1;
        var email = "test@example.com";
        var role = "User";
        var token = _jwtService.GenerateToken(userId, email, role);
        
        // Tamper with the token
        var tamperedToken = token.Substring(0, token.Length - 5) + "XXXXX";

        // Act
        var result = _jwtService.ValidateToken(tamperedToken);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GenerateToken_MissingJwtKey_ShouldThrowException()
    {
        // Arrange
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["Jwt:Key"]).Returns((string?)null);
        var service = new JwtService(configMock.Object);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => 
            service.GenerateToken(1, "test@example.com", "User"));
    }

    [TestCase(1, "user1@test.com", "User")]
    [TestCase(999, "admin@test.com", "Admin")]
    [TestCase(0, "zero@test.com", "Guest")]
    public void GenerateToken_VariousInputs_ShouldGenerateValidTokens(int userId, string email, string role)
    {
        // Act
        var token = _jwtService.GenerateToken(userId, email, role);
        var validatedUserId = _jwtService.ValidateToken(token);

        // Assert
        Assert.That(validatedUserId, Is.Not.Null);
        Assert.That(validatedUserId!.Value, Is.EqualTo(userId));
    }

    [Test]
    public void GenerateToken_DifferentUsers_ShouldGenerateDifferentTokens()
    {
        // Arrange & Act
        var token1 = _jwtService.GenerateToken(1, "user1@test.com", "User");
        var token2 = _jwtService.GenerateToken(2, "user2@test.com", "User");

        // Assert
        Assert.That(token1, Is.Not.EqualTo(token2));
    }
}
