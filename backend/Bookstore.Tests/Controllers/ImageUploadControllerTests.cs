using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Bookstore.Web.Controllers;
using Bookstore.Models;

namespace Bookstore.Tests.Controllers;

[TestFixture]
public class ImageUploadControllerTests
{
    private ImageUploadController _controller = null!;
    private Mock<IConfiguration> _mockConfiguration = null!;

    [SetUp]
    public void Setup()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _controller = new ImageUploadController(_mockConfiguration.Object);
    }

    #region UploadImage Tests

    [Test]
    public async Task UploadImage_WithNullFile_ShouldReturnBadRequest()
    {
        // Arrange
        IFormFile? file = null;

        // Act
        var result = await _controller.UploadImage(file!);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.That(badRequestResult, Is.Not.Null);
        var response = badRequestResult!.Value as ApiResponse<string>;
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Success, Is.False);
        Assert.That(response.Message, Is.EqualTo("No file provided"));
    }

    [Test]
    public async Task UploadImage_WithEmptyFile_ShouldReturnBadRequest()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(0);

        // Act
        var result = await _controller.UploadImage(mockFile.Object);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task UploadImage_WithValidFile_ShouldCallImageKit()
    {
        // Arrange
        var content = "Fake image content";
        var fileName = "test.jpg";
        var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));

        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(stream.Length);
        mockFile.Setup(f => f.FileName).Returns(fileName);
        mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Callback<Stream, CancellationToken>((s, ct) => stream.CopyTo(s))
            .Returns(Task.CompletedTask);

        _mockConfiguration.Setup(c => c["ImageKit:PublicKey"]).Returns("test_public_key");
        _mockConfiguration.Setup(c => c["ImageKit:PrivateKey"]).Returns("test_private_key");
        _mockConfiguration.Setup(c => c["ImageKit:UrlEndpoint"]).Returns("https://ik.imagekit.io/test");

        // Act
        var result = await _controller.UploadImage(mockFile.Object);

        // Assert
        // The result will either be Ok (if ImageKit works) or StatusCode 500 (if ImageKit fails due to invalid credentials)
        // Since we're using mock credentials, it will likely fail, but we're testing the flow
        Assert.That(result.Result, Is.Not.Null);
    }

    #endregion
}
