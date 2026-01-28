using Bookstore.Business.Services.Users.Commands;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Bookstore.Tests.Validators;

[TestFixture]
public class RegisterUserCommandValidatorTests
{
    private RegisterUserCommandValidator _validator = null!;

    [SetUp]
    public void Setup()
    {
        _validator = new RegisterUserCommandValidator();
    }

    [Test]
    public void Validate_WithValidCommand_ShouldNotHaveErrors()
    {
        var command = new RegisterUserCommand(
            FullName: "John Doe",
            Email: "john@example.com",
            Password: "password123",
            Phone: "1234567890"
        );

        var result = _validator.TestValidate(command);
        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void Validate_WithEmptyFullName_ShouldHaveError()
    {
        var command = new RegisterUserCommand("", "email@test.com", "password", "1234567890");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Test]
    public void Validate_WithInvalidEmail_ShouldHaveError()
    {
        var command = new RegisterUserCommand("John Doe", "invalid-email", "password", "1234567890");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Test]
    public void Validate_WithShortPassword_ShouldHaveError()
    {
        var command = new RegisterUserCommand("John Doe", "email@test.com", "12345", "1234567890");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Test]
    public void Validate_WithInvalidPhone_ShouldHaveError()
    {
        var command = new RegisterUserCommand("John Doe", "email@test.com", "password", "123");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Test]
    public void Validate_WithNonNumericPhone_ShouldHaveError()
    {
        var command = new RegisterUserCommand("John Doe", "email@test.com", "password", "abcd123456");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }
}
