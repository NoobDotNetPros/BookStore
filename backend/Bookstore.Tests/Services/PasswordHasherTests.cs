using Bookstore.Business.Services;
using NUnit.Framework;

namespace Bookstore.Tests.Services;

[TestFixture]
public class PasswordHasherTests
{
    private PasswordHasher _passwordHasher = null!;

    [SetUp]
    public void SetUp()
    {
        _passwordHasher = new PasswordHasher();
    }

    [Test]
    public void HashPassword_ShouldReturnNonEmptyString()
    {
        // Arrange
        var password = "TestPassword123!";

        // Act
        var hash = _passwordHasher.HashPassword(password);

        // Assert
        Assert.That(string.IsNullOrEmpty(hash), Is.False);
    }

    [Test]
    public void HashPassword_ShouldReturnBase64EncodedString()
    {
        // Arrange
        var password = "TestPassword123!";

        // Act
        var hash = _passwordHasher.HashPassword(password);

        // Assert - Should not throw when converting from Base64
        var bytes = Convert.FromBase64String(hash);
        Assert.That(bytes, Is.Not.Null);
        Assert.That(bytes.Length, Is.GreaterThan(0));
    }

    [Test]
    public void HashPassword_SamePlaintext_ShouldReturnDifferentHashes()
    {
        // Arrange
        var password = "TestPassword123!";

        // Act
        var hash1 = _passwordHasher.HashPassword(password);
        var hash2 = _passwordHasher.HashPassword(password);

        // Assert - Different salts should result in different hashes
        Assert.That(hash1, Is.Not.EqualTo(hash2));
    }

    [Test]
    public void VerifyPassword_CorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        var password = "TestPassword123!";
        var hash = _passwordHasher.HashPassword(password);

        // Act
        var result = _passwordHasher.VerifyPassword(password, hash);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void VerifyPassword_IncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        var password = "TestPassword123!";
        var wrongPassword = "WrongPassword456!";
        var hash = _passwordHasher.HashPassword(password);

        // Act
        var result = _passwordHasher.VerifyPassword(wrongPassword, hash);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void VerifyPassword_EmptyPassword_ShouldReturnFalse()
    {
        // Arrange
        var password = "TestPassword123!";
        var hash = _passwordHasher.HashPassword(password);

        // Act
        var result = _passwordHasher.VerifyPassword("", hash);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void VerifyPassword_CaseSensitive_ShouldReturnFalse()
    {
        // Arrange
        var password = "TestPassword123!";
        var wrongCasePassword = "testpassword123!";
        var hash = _passwordHasher.HashPassword(password);

        // Act
        var result = _passwordHasher.VerifyPassword(wrongCasePassword, hash);

        // Assert
        Assert.That(result, Is.False);
    }

    [TestCase("short")]
    [TestCase("a")]
    [TestCase("12345678901234567890123456789012345678901234567890")] // 50 chars
    [TestCase("P@ssw0rd!#$%^&*()")]
    [TestCase("パスワード")] // Unicode
    public void HashPassword_VariousPasswords_ShouldHashAndVerify(string password)
    {
        // Arrange & Act
        var hash = _passwordHasher.HashPassword(password);
        var result = _passwordHasher.VerifyPassword(password, hash);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void HashPassword_HashLength_ShouldBeConsistent()
    {
        // Arrange
        var password1 = "short";
        var password2 = "a very long password with many characters";

        // Act
        var hash1 = _passwordHasher.HashPassword(password1);
        var hash2 = _passwordHasher.HashPassword(password2);

        // Assert - Hash length should be consistent (salt + hash = 16 + 32 = 48 bytes in base64)
        Assert.That(hash1.Length, Is.EqualTo(hash2.Length));
    }
}
