using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Bookstore.Web.Controllers;
using Bookstore.Business.Interfaces;
using Bookstore.Models.Entities;

namespace Bookstore.Tests.Controllers;

[TestFixture]
public class AddressControllerTests
{
    private AddressController _controller = null!;
    private Mock<IUserRepository> _mockUserRepository = null!;
    private Mock<IUnitOfWork> _mockUnitOfWork = null!;

    [SetUp]
    public void Setup()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _controller = new AddressController(
            _mockUserRepository.Object,
            _mockUnitOfWork.Object);

        SetupUserContext(userId: 1);
    }

    private void SetupUserContext(int userId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim("sub", userId.ToString())
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    #region GetUserProfile Tests

    [Test]
    public async Task GetUserProfile_WithValidUser_ShouldReturnOk()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            FullName = "John Doe",
            Email = "john@test.com",
            Phone = "1234567890",
            Addresses = new List<Address>()
        };

        _mockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _controller.GetUserProfile();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetUserProfile_WithNonExistentUser_ShouldReturnNotFound()
    {
        // Arrange
        _mockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _controller.GetUserProfile();

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetUserProfile_WithoutAuth_ShouldReturnUnauthorized()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        // Act
        var result = await _controller.GetUserProfile();

        // Assert
        Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
    }

    #endregion

    #region UpdateUserProfile Tests

    [Test]
    public async Task UpdateUserProfile_WithValidRequest_ShouldReturnOk()
    {
        // Arrange
        var request = new UpdateProfileRequest(
            FullName: "Updated Name",
            Email: "updated@test.com",
            Phone: "9876543210");

        var user = new User
        {
            Id = 1,
            FullName = "John Doe",
            Email = "john@test.com",
            Phone = "1234567890"
        };

        _mockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockUserRepository
            .Setup(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.UpdateUserProfile(request);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task UpdateUserProfile_WithNonExistentUser_ShouldReturnNotFound()
    {
        // Arrange
        var request = new UpdateProfileRequest(
            FullName: "Updated Name",
            Email: "updated@test.com",
            Phone: "9876543210");

        _mockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _controller.UpdateUserProfile(request);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    #endregion

    #region UpdateCustomerDetails Tests

    [Test]
    public async Task UpdateCustomerDetails_WithValidRequest_ShouldReturnOk()
    {
        // Arrange
        var request = new AddressRequest(
            AddressType: "Home",
            FullAddress: "123 Main St",
            City: "Test City",
            State: "TS");

        var user = new User
        {
            Id = 1,
            FullName = "John Doe",
            Email = "john@test.com",
            Addresses = new List<Address>()
        };

        _mockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockUserRepository
            .Setup(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.UpdateCustomerDetails(request);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task UpdateCustomerDetails_WithNonExistentUser_ShouldReturnNotFound()
    {
        // Arrange
        var request = new AddressRequest(
            AddressType: "Home",
            FullAddress: "123 Main St",
            City: "Test City",
            State: "TS");

        _mockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _controller.UpdateCustomerDetails(request);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    #endregion

    #region AddAddress Tests

    [Test]
    public async Task AddAddress_WithValidRequest_ShouldReturnCreated()
    {
        // Arrange
        var request = new AddressRequest(
            AddressType: "Work",
            FullAddress: "456 Office St",
            City: "Business City",
            State: "BC");

        var user = new User
        {
            Id = 1,
            FullName = "John Doe",
            Email = "john@test.com",
            Addresses = new List<Address>()
        };

        _mockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockUserRepository
            .Setup(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.AddAddress(request);

        // Assert
        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
    }

    [Test]
    public async Task AddAddress_WithoutAuth_ShouldReturnUnauthorized()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        var request = new AddressRequest(
            AddressType: "Work",
            FullAddress: "456 Office St",
            City: "Business City",
            State: "BC");

        // Act
        var result = await _controller.AddAddress(request);

        // Assert
        Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
    }

    #endregion

    #region UpdateAddress Tests

    [Test]
    public async Task UpdateAddress_WithValidRequest_ShouldReturnOk()
    {
        // Arrange
        int addressId = 1;
        var request = new AddressRequest(
            AddressType: "Home",
            FullAddress: "Updated Address",
            City: "Updated City",
            State: "UC");

        var existingAddress = new Address
        {
            Id = 1,
            UserId = 1,
            AddressType = "Home",
            FullAddress = "123 Old St",
            City = "Old City",
            State = "OC"
        };

        var user = new User
        {
            Id = 1,
            FullName = "John Doe",
            Email = "john@test.com",
            Addresses = new List<Address> { existingAddress }
        };

        _mockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockUserRepository
            .Setup(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.UpdateAddress(addressId, request);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task UpdateAddress_WithNonExistentAddress_ShouldReturnNotFound()
    {
        // Arrange
        int addressId = 999;
        var request = new AddressRequest(
            AddressType: "Home",
            FullAddress: "Updated Address",
            City: "Updated City",
            State: "UC");

        var user = new User
        {
            Id = 1,
            FullName = "John Doe",
            Email = "john@test.com",
            Addresses = new List<Address>()
        };

        _mockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _controller.UpdateAddress(addressId, request);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    #endregion

    #region DeleteAddress Tests

    [Test]
    public async Task DeleteAddress_WithValidAddress_ShouldReturnOk()
    {
        // Arrange
        int addressId = 1;
        var existingAddress = new Address
        {
            Id = 1,
            UserId = 1,
            AddressType = "Home",
            FullAddress = "123 Test St",
            City = "Test City",
            State = "TC"
        };

        var user = new User
        {
            Id = 1,
            FullName = "John Doe",
            Email = "john@test.com",
            Addresses = new List<Address> { existingAddress }
        };

        _mockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockUserRepository
            .Setup(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.DeleteAddress(addressId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task DeleteAddress_WithNonExistentAddress_ShouldReturnNotFound()
    {
        // Arrange
        int addressId = 999;

        var user = new User
        {
            Id = 1,
            FullName = "John Doe",
            Email = "john@test.com",
            Addresses = new List<Address>()
        };

        _mockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _controller.DeleteAddress(addressId);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    #endregion
}
