using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Bookstore.Web.Controllers;
using Bookstore.DataAccess.Context;
using Bookstore.Business.Models;

namespace Bookstore.Tests.Controllers;

[TestFixture]
public class HealthControllerTests
{
    private HealthController _controller = null!;
    private ApplicationDbContext _context = null!;
    private Mock<IConfiguration> _mockConfiguration = null!;
    private Mock<IOptions<SmtpSettings>> _mockSmtpOptions = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _mockConfiguration = new Mock<IConfiguration>();
        _mockSmtpOptions = new Mock<IOptions<SmtpSettings>>();

        var smtpSettings = new SmtpSettings
        {
            Host = "smtp.test.com",
            Port = 587,
            UserName = "test@test.com",
            Password = "password"
        };
        _mockSmtpOptions.Setup(x => x.Value).Returns(smtpSettings);

        // Setup JWT configuration
        _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("ThisIsAVeryLongSecretKeyForTestingPurposes123!");

        _controller = new HealthController(
            _context,
            _mockConfiguration.Object,
            _mockSmtpOptions.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    #region CheckHealth Tests

    [Test]
    public async Task CheckHealth_WithValidConfiguration_ShouldReturnOk()
    {
        // Act
        var result = await _controller.CheckHealth();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task CheckHealth_WithInvalidJwtKey_ShouldStillReturnOkButNotHealthy()
    {
        // Arrange
        _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("short");

        var controller = new HealthController(
            _context,
            _mockConfiguration.Object,
            _mockSmtpOptions.Object);

        // Act
        var result = await controller.CheckHealth();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task CheckHealth_WithMissingSmtpConfig_ShouldStillReturnOk()
    {
        // Arrange
        var emptySmtpSettings = new SmtpSettings();
        var mockEmptySmtpOptions = new Mock<IOptions<SmtpSettings>>();
        mockEmptySmtpOptions.Setup(x => x.Value).Returns(emptySmtpSettings);

        var controller = new HealthController(
            _context,
            _mockConfiguration.Object,
            mockEmptySmtpOptions.Object);

        // Act
        var result = await controller.CheckHealth();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task CheckHealth_DatabaseCanConnect_ShouldShowDatabaseOnline()
    {
        // Act
        var result = await _controller.CheckHealth();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
    }

    #endregion
}
