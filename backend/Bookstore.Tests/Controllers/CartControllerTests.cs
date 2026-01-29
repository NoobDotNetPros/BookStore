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
public class CartControllerTests
{
    private CartController _controller = null!;
    private Mock<ICartRepository> _mockCartRepository = null!;
    private Mock<IBookRepository> _mockBookRepository = null!;
    private Mock<IUnitOfWork> _mockUnitOfWork = null!;

    [SetUp]
    public void Setup()
    {
        _mockCartRepository = new Mock<ICartRepository>();
        _mockBookRepository = new Mock<IBookRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _controller = new CartController(
            _mockCartRepository.Object,
            _mockBookRepository.Object,
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

    #region AddToCart Tests

    [Test]
    public async Task AddToCart_WithValidBook_ShouldReturnOk()
    {
        // Arrange
        var request = new AddCartItemRequest(BookId: 1, Quantity: 2);
        var book = new Book { Id = 1, BookName = "Test Book", Price = 100 };
        var cartItem = new CartItem { Id = 1, UserId = 1, BookId = 1, Quantity = 2, IsWishlist = false };

        _mockBookRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        _mockCartRepository
            .Setup(x => x.GetUserCartItemByBookIdAsync(1, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CartItem?)null);

        _mockCartRepository
            .Setup(x => x.AddAsync(It.IsAny<CartItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cartItem);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mockCartRepository
            .Setup(x => x.GetUserCartItemsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CartItem> { cartItem });

        // Act
        var result = await _controller.AddToCart(request);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult!.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public async Task AddToCart_WithNonExistentBook_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new AddCartItemRequest(BookId: 999, Quantity: 1);

        _mockBookRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        // Act
        var result = await _controller.AddToCart(request);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task AddToCart_WithoutAuth_ShouldReturnUnauthorized()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        var request = new AddCartItemRequest(BookId: 1, Quantity: 1);

        // Act
        var result = await _controller.AddToCart(request);

        // Assert
        Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
    }

    [Test]
    public async Task AddToCart_WithExistingItemInCart_ShouldUpdateQuantity()
    {
        // Arrange
        var request = new AddCartItemRequest(BookId: 1, Quantity: 2);
        var book = new Book { Id = 1, BookName = "Test Book", Price = 100 };
        var existingCartItem = new CartItem { Id = 1, UserId = 1, BookId = 1, Quantity = 3, IsWishlist = false };

        _mockBookRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        _mockCartRepository
            .Setup(x => x.GetUserCartItemByBookIdAsync(1, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCartItem);

        _mockCartRepository
            .Setup(x => x.UpdateAsync(It.IsAny<CartItem>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mockCartRepository
            .Setup(x => x.GetUserCartItemsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CartItem> { existingCartItem });

        // Act
        var result = await _controller.AddToCart(request);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    #endregion

    #region UpdateQuantity Tests

    [Test]
    public async Task UpdateQuantity_WithValidItem_ShouldReturnOk()
    {
        // Arrange
        int cartItemId = 1;
        var request = new UpdateQuantityRequest(QuantityToBuy: 5);
        var book = new Book { Id = 1, BookName = "Test Book", Price = 100 };
        var cartItem = new CartItem { Id = 1, UserId = 1, BookId = 1, Quantity = 2, IsWishlist = false, Book = book };

        _mockCartRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cartItem);

        _mockCartRepository
            .Setup(x => x.UpdateAsync(It.IsAny<CartItem>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mockCartRepository
            .Setup(x => x.GetUserCartItemsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CartItem> { cartItem });

        // Act
        var result = await _controller.UpdateQuantity(cartItemId, request);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task UpdateQuantity_WithNonExistentItem_ShouldReturnNotFound()
    {
        // Arrange
        int cartItemId = 999;
        var request = new UpdateQuantityRequest(QuantityToBuy: 5);

        _mockCartRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CartItem?)null);

        // Act
        var result = await _controller.UpdateQuantity(cartItemId, request);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task UpdateQuantity_WithDifferentUserItem_ShouldReturnNotFound()
    {
        // Arrange
        int cartItemId = 1;
        var request = new UpdateQuantityRequest(QuantityToBuy: 5);
        var cartItem = new CartItem { Id = 1, UserId = 999, BookId = 1, Quantity = 2, IsWishlist = false };

        _mockCartRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cartItem);

        // Act
        var result = await _controller.UpdateQuantity(cartItemId, request);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    #endregion

    #region RemoveFromCart Tests

    [Test]
    public async Task RemoveFromCart_WithValidItem_ShouldReturnOk()
    {
        // Arrange
        int cartItemId = 1;
        var book = new Book { Id = 1, BookName = "Test Book", Price = 100 };
        var cartItem = new CartItem { Id = 1, UserId = 1, BookId = 1, Quantity = 2, IsWishlist = false, Book = book };

        _mockCartRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cartItem);

        _mockCartRepository
            .Setup(x => x.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mockCartRepository
            .Setup(x => x.GetUserCartItemsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CartItem>());

        // Act
        var result = await _controller.RemoveFromCart(cartItemId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task RemoveFromCart_WithNonExistentItem_ShouldReturnNotFound()
    {
        // Arrange
        int cartItemId = 999;

        _mockCartRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CartItem?)null);

        // Act
        var result = await _controller.RemoveFromCart(cartItemId);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    #endregion

    #region GetCartItems Tests

    [Test]
    public async Task GetCartItems_ShouldReturnOkWithItems()
    {
        // Arrange
        var cartItems = new List<CartItem>
        {
            new CartItem { Id = 1, UserId = 1, BookId = 1, Quantity = 2, IsWishlist = false },
            new CartItem { Id = 2, UserId = 1, BookId = 2, Quantity = 1, IsWishlist = false }
        };

        _mockCartRepository
            .Setup(x => x.GetUserCartItemsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cartItems);

        // Act
        var result = await _controller.GetCartItems();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetCartItems_WithEmptyCart_ShouldReturnOkWithEmptyList()
    {
        // Arrange
        _mockCartRepository
            .Setup(x => x.GetUserCartItemsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CartItem>());

        // Act
        var result = await _controller.GetCartItems();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetCartItems_WithoutAuth_ShouldReturnUnauthorized()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        // Act
        var result = await _controller.GetCartItems();

        // Assert
        Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
    }

    #endregion
}
