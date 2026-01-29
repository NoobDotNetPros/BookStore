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
public class OrderControllerTests
{
    private OrderController _controller = null!;
    private Mock<IOrderRepository> _mockOrderRepository = null!;
    private Mock<ICartRepository> _mockCartRepository = null!;
    private Mock<IUserRepository> _mockUserRepository = null!;
    private Mock<IUnitOfWork> _mockUnitOfWork = null!;

    [SetUp]
    public void Setup()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockCartRepository = new Mock<ICartRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _controller = new OrderController(
            _mockOrderRepository.Object,
            _mockUserRepository.Object,
            _mockCartRepository.Object,
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

    #region CreateOrder Tests

    [Test]
    public async Task CreateOrder_WithValidRequest_ShouldReturnOk()
    {
        // Arrange
        var orderItems = new List<OrderItemRequest>
        {
            new OrderItemRequest("1", "Book 1", 2, 100m)
        };
        var request = new CreateOrderRequest(orderItems);
        var user = new User 
        { 
            Id = 1, 
            FullName = "Test User", 
            Email = "test@test.com",
            Addresses = new List<Address>
            {
                new Address { Id = 1, UserId = 1, FullAddress = "123 Test St", City = "Test City", State = "TS" }
            }
        };

        _mockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockOrderRepository
            .Setup(x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Order { Id = 1, UserId = 1, TotalAmount = 200 });

        _mockCartRepository
            .Setup(x => x.ClearCartAsync(1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.BeginTransactionAsync())
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mockUnitOfWork
            .Setup(x => x.CommitTransactionAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateOrder(request);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task CreateOrder_WithoutAuth_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        var orderItems = new List<OrderItemRequest>
        {
            new OrderItemRequest("1", "Book 1", 2, 100m)
        };
        var request = new CreateOrderRequest(orderItems);

        // Act & Assert
        Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _controller.CreateOrder(request));
    }

    #endregion

    #region GetUserOrders Tests

    [Test]
    public async Task GetUserOrders_ShouldReturnOkWithOrders()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order 
            { 
                Id = 1, 
                UserId = 1, 
                TotalAmount = 200, 
                Status = OrderStatus.Pending,
                OrderItems = new List<OrderItem>()
            },
            new Order 
            { 
                Id = 2, 
                UserId = 1, 
                TotalAmount = 150, 
                Status = OrderStatus.Delivered,
                OrderItems = new List<OrderItem>()
            }
        };

        _mockOrderRepository
            .Setup(x => x.GetUserOrdersAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(orders);

        // Act
        var result = await _controller.GetUserOrders();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetUserOrders_WithNoOrders_ShouldReturnOkWithEmptyList()
    {
        // Arrange
        _mockOrderRepository
            .Setup(x => x.GetUserOrdersAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Order>());

        // Act
        var result = await _controller.GetUserOrders();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    #endregion

    #region GetOrderById Tests

    [Test]
    public async Task GetOrderById_WithValidOrder_ShouldReturnOk()
    {
        // Arrange
        int orderId = 1;
        var order = new Order
        {
            Id = 1,
            UserId = 1,
            TotalAmount = 200,
            Status = OrderStatus.Pending,
            OrderItems = new List<OrderItem>
            {
                new OrderItem { Id = 1, OrderId = 1, BookId = 1, Quantity = 2, Price = 100 }
            }
        };

        _mockOrderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        // Act
        var result = await _controller.GetOrderById(orderId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetOrderById_WithNonExistentOrder_ShouldReturnNotFound()
    {
        // Arrange
        int orderId = 999;

        _mockOrderRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order?)null);

        // Act
        var result = await _controller.GetOrderById(orderId);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetOrderById_WithOtherUserOrder_ShouldReturnNotFound()
    {
        // Arrange
        int orderId = 1;
        var order = new Order
        {
            Id = 1,
            UserId = 999, // Different user
            TotalAmount = 200,
            Status = OrderStatus.Pending
        };

        _mockOrderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        // Act
        var result = await _controller.GetOrderById(orderId);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    #endregion
}
