using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Bookstore.Web.Controllers;
using Bookstore.Business.Services.Users.Commands;
using Bookstore.Models.DTOs;

namespace Bookstore.Tests.Controllers;

[TestFixture]
public class UserAuthControllerTests
{
    private UserAuthController _controller = null!;
    private Mock<IMediator> _mockMediator = null!;

    [SetUp]
    public void Setup()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new UserAuthController(_mockMediator.Object);
    }

    #region RegisterUser Tests

    [Test]
    public async Task RegisterUser_WithValidRequest_ShouldReturnOk()
    {
        // Arrange
        var command = new RegisterUserCommand(
            FullName: "Test User",
            Email: "test@test.com",
            Password: "Password123!",
            Phone: "1234567890");

        _mockMediator
            .Setup(x => x.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.RegisterUser(command);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
    }

    [Test]
    public async Task RegisterUser_WithDuplicateEmail_ShouldReturnBadRequest()
    {
        // Arrange
        var command = new RegisterUserCommand(
            FullName: "Test User",
            Email: "existing@test.com",
            Password: "Password123!",
            Phone: "1234567890");

        _mockMediator
            .Setup(x => x.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Email already exists"));

        // Act
        var result = await _controller.RegisterUser(command);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    #endregion

    #region LoginUser Tests

    [Test]
    public async Task LoginUser_WithValidCredentials_ShouldReturnOkWithToken()
    {
        // Arrange
        var command = new LoginCommand(Email: "test@test.com", Password: "Password123!");
        var loginResponse = new LoginResponseDto
        {
            Token = "jwt-token",
            UserId = 1,
            Email = "test@test.com",
            FullName = "Test User",
            Role = "Customer"
        };

        _mockMediator
            .Setup(x => x.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(loginResponse);

        // Act
        var result = await _controller.LoginUser(command);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task LoginUser_WithInvalidCredentials_ShouldReturnUnauthorized()
    {
        // Arrange
        var command = new LoginCommand(Email: "test@test.com", Password: "wrongpassword");

        _mockMediator
            .Setup(x => x.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new UnauthorizedAccessException("Invalid credentials"));

        // Act
        var result = await _controller.LoginUser(command);

        // Assert
        Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
    }

    #endregion

    #region ForgotPassword Tests

    [Test]
    public async Task ForgotPassword_WithValidEmail_ShouldReturnOk()
    {
        // Arrange
        var command = new ForgotPasswordCommand(Email: "test@test.com");
        var response = new ForgotPasswordResponse(Success: true, Message: "OTP sent successfully", Email: "test@test.com");

        _mockMediator
            .Setup(x => x.Send(It.IsAny<ForgotPasswordCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.ForgotPassword(command);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task ForgotPassword_WithInvalidEmail_ShouldReturnBadRequest()
    {
        // Arrange
        var command = new ForgotPasswordCommand(Email: "nonexistent@test.com");
        var response = new ForgotPasswordResponse(Success: false, Message: "User not found");

        _mockMediator
            .Setup(x => x.Send(It.IsAny<ForgotPasswordCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.ForgotPassword(command);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    #endregion

    #region VerifyOtp Tests

    [Test]
    public async Task VerifyOtp_WithValidOtp_ShouldReturnOk()
    {
        // Arrange
        var command = new VerifyOtpCommand(Email: "test@test.com", Otp: "123456");
        var response = new VerifyOtpResponse(Success: true, Message: "OTP verified successfully", ResetToken: "reset-token");

        _mockMediator
            .Setup(x => x.Send(It.IsAny<VerifyOtpCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.VerifyOtp(command);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task VerifyOtp_WithInvalidOtp_ShouldReturnBadRequest()
    {
        // Arrange
        var command = new VerifyOtpCommand(Email: "test@test.com", Otp: "000000");
        var response = new VerifyOtpResponse(Success: false, Message: "Invalid OTP");

        _mockMediator
            .Setup(x => x.Send(It.IsAny<VerifyOtpCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.VerifyOtp(command);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    #endregion

    #region ResendOtp Tests

    [Test]
    public async Task ResendOtp_WithValidEmail_ShouldReturnOk()
    {
        // Arrange
        var command = new ResendOtpCommand(Email: "test@test.com");
        var response = new ResendOtpResponse(Success: true, Message: "OTP resent successfully");

        _mockMediator
            .Setup(x => x.Send(It.IsAny<ResendOtpCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.ResendOtp(command);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task ResendOtp_WithRateLimitExceeded_ShouldReturnBadRequest()
    {
        // Arrange
        var command = new ResendOtpCommand(Email: "test@test.com");
        var response = new ResendOtpResponse(Success: false, Message: "Please wait before requesting another OTP", WaitTimeSeconds: 60);

        _mockMediator
            .Setup(x => x.Send(It.IsAny<ResendOtpCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.ResendOtp(command);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    #endregion
}
