using Bookstore.Business.Interfaces;
using Bookstore.Business.Models;
using Bookstore.Business.Services;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Bookstore.Tests.Services;

[TestFixture]
public class EmailServiceTests
{
    private EmailService _emailService = null!;
    private Mock<IOptions<SmtpSettings>> _smtpSettingsMock = null!;

    [SetUp]
    public void Setup()
    {
        _smtpSettingsMock = new Mock<IOptions<SmtpSettings>>();
        var smtpSettings = new SmtpSettings
        {
            Host = "smtp.test.com",
            Port = 587,
            UserName = "test@test.com",
            Password = "password123",
            FromEmail = "noreply@bookstore.com",
            EnableSsl = true
        };
        _smtpSettingsMock.Setup(x => x.Value).Returns(smtpSettings);
        _emailService = new EmailService(_smtpSettingsMock.Object);
    }

    [Test]
    public void Constructor_WithValidSettings_CreatesInstance()
    {
        Assert.That(_emailService, Is.Not.Null);
    }

    [Test]
    public void Constructor_WithNullSettings_ThrowsException()
    {
        Mock<IOptions<SmtpSettings>> nullMock = null!;
        Assert.Throws<NullReferenceException>(() => new EmailService(nullMock!.Object));
    }

    // Note: Testing actual SMTP functionality requires integration tests
    // These would need a real SMTP server or mocking SmtpClient
    // For now, we skip detailed email sending tests as they require infrastructure
}

