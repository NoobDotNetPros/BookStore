using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Bookstore.Web.Controllers.Admin;
using Bookstore.Business.Services.Users.Commands;
using Bookstore.Business.Services.Books.Commands;
using Bookstore.Business.Interfaces;
using Bookstore.Models.Entities;

namespace Bookstore.Tests.Controllers.Admin;

#region AdminAuthController Tests

[TestFixture]
public class AdminAuthControllerTests
{
    private AdminAuthController _controller = null!;
    private Mock<IMediator> _mockMediator = null!;

    [SetUp]
    public void Setup()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new AdminAuthController(_mockMediator.Object);
    }

    [Test]
    public async Task RegisterAdmin_WithValidCommand_ShouldReturnOk()
    {
        // Arrange
        var command = new RegisterUserCommand(
            FullName: "Admin User",
            Email: "admin@test.com",
            Password: "Admin123!",
            Phone: "1234567890");

        _mockMediator
            .Setup(x => x.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.RegisterAdmin(command);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task LoginAdmin_WithValidCredentials_ShouldReturnOk()
    {
        // Arrange
        var request = new LoginRequest(Email: "admin@test.com", Password: "Admin123!");

        // Act
        var result = await _controller.LoginAdmin(request);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
}

#endregion

#region AdminBookController Tests

[TestFixture]
public class AdminBookControllerTests
{
    private AdminBookController _controller = null!;
    private Mock<IMediator> _mockMediator = null!;

    [SetUp]
    public void Setup()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new AdminBookController(_mockMediator.Object);
    }

    [Test]
    public async Task AddBook_WithValidCommand_ShouldReturnOk()
    {
        // Arrange
        var command = new CreateBookCommand(
            BookName: "New Book",
            Author: "New Author",
            Description: "Description",
            Quantity: 10,
            Price: 100m,
            DiscountPrice: 80m);

        _mockMediator
            .Setup(x => x.Send(It.IsAny<CreateBookCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.AddBook(command);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
    }

    [Test]
    public async Task UpdateBook_WithValidRequest_ShouldReturnOk()
    {
        // Arrange
        int productId = 1;
        var request = new UpdateBookRequest(
            BookName: "Updated Book",
            Author: "Updated Author",
            Description: "Updated Description",
            Quantity: 15,
            Price: 120m,
            DiscountPrice: 100m);

        _mockMediator
            .Setup(x => x.Send(It.IsAny<UpdateBookCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.UpdateBook(productId, request);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task DeleteBook_WithValidId_ShouldReturnOk()
    {
        // Arrange
        int productId = 1;

        _mockMediator
            .Setup(x => x.Send(It.IsAny<DeleteBookCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.DeleteBook(productId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task AddBook_ShouldCallMediatorSend()
    {
        // Arrange
        var command = new CreateBookCommand(
            BookName: "New Book",
            Author: "New Author",
            Description: "Description",
            Quantity: 10,
            Price: 100m,
            DiscountPrice: 80m);

        _mockMediator
            .Setup(x => x.Send(It.IsAny<CreateBookCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _controller.AddBook(command);

        // Assert
        _mockMediator.Verify(x => x.Send(It.IsAny<CreateBookCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}

#endregion

#region AdminOrderController Tests

[TestFixture]
public class AdminOrderControllerTests
{
    private AdminOrderController _controller = null!;
    private Mock<IOrderRepository> _mockOrderRepository = null!;

    [SetUp]
    public void Setup()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _controller = new AdminOrderController(_mockOrderRepository.Object);
    }

    [Test]
    public async Task GetAllOrders_ShouldReturnOkWithOrders()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order { Id = 1, UserId = 1, TotalAmount = 200, Status = OrderStatus.Pending },
            new Order { Id = 2, UserId = 2, TotalAmount = 150, Status = OrderStatus.Delivered }
        };

        _mockOrderRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(orders);

        // Act
        var result = await _controller.GetAllOrders();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetAllOrders_WithEmptyList_ShouldReturnOkWithEmptyList()
    {
        // Arrange
        _mockOrderRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Order>());

        // Act
        var result = await _controller.GetAllOrders();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
}

#endregion
