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
public class WishlistControllerTests
{
    private WishlistController _controller = null!;
    private Mock<ICartRepository> _mockCartRepository = null!;
    private Mock<IBookRepository> _mockBookRepository = null!;
    private Mock<IUnitOfWork> _mockUnitOfWork = null!;

    [SetUp]
    public void Setup()
    {
        _mockCartRepository = new Mock<ICartRepository>();
        _mockBookRepository = new Mock<IBookRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _controller = new WishlistController(
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

    #region AddToWishlist Tests

    [Test]
    public async Task AddToWishlist_WithValidBook_ShouldReturnOk()
    {
        // Arrange
        int productId = 1;
        var book = new Book { Id = 1, BookName = "Test Book", Price = 100, DiscountPrice = 80 };

        _mockBookRepository
            .Setup(x => x.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        _mockCartRepository
            .Setup(x => x.GetUserWishlistItemsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CartItem>());

        _mockCartRepository
            .Setup(x => x.AddAsync(It.IsAny<CartItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CartItem { Id = 1, UserId = 1, BookId = 1, IsWishlist = true });

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.AddToWishlist(productId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task AddToWishlist_WithNonExistentBook_ShouldReturnBadRequest()
    {
        // Arrange
        int productId = 999;

        _mockBookRepository
            .Setup(x => x.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        // Act
        var result = await _controller.AddToWishlist(productId);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task AddToWishlist_WithExistingItemInWishlist_ShouldReturnBadRequest()
    {
        // Arrange
        int productId = 1;
        var book = new Book { Id = 1, BookName = "Test Book", Price = 100 };
        var existingItem = new CartItem { Id = 1, UserId = 1, BookId = 1, IsWishlist = true };

        _mockBookRepository
            .Setup(x => x.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        _mockCartRepository
            .Setup(x => x.GetUserWishlistItemsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CartItem> { existingItem });

        // Act
        var result = await _controller.AddToWishlist(productId);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task AddToWishlist_WithoutAuth_ShouldReturnUnauthorized()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        int productId = 1;

        // Act
        var result = await _controller.AddToWishlist(productId);

        // Assert
        Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
    }

    #endregion

    #region RemoveFromWishlist Tests

    [Test]
    public async Task RemoveFromWishlist_WithValidItem_ShouldReturnOk()
    {
        // Arrange
        int productId = 1;
        var wishlistItem = new CartItem { Id = 1, UserId = 1, BookId = 1, IsWishlist = true };

        _mockCartRepository
            .Setup(x => x.GetUserWishlistItemsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CartItem> { wishlistItem });

        _mockCartRepository
            .Setup(x => x.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.RemoveFromWishlist(productId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task RemoveFromWishlist_WithNonExistentItem_ShouldReturnNotFound()
    {
        // Arrange
        int productId = 999;

        _mockCartRepository
            .Setup(x => x.GetUserWishlistItemsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CartItem>());

        // Act
        var result = await _controller.RemoveFromWishlist(productId);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task RemoveFromWishlist_WithoutAuth_ShouldReturnUnauthorized()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        int productId = 1;

        // Act
        var result = await _controller.RemoveFromWishlist(productId);

        // Assert
        Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
    }

    #endregion

    #region GetWishlistItems Tests

    [Test]
    public async Task GetWishlistItems_ShouldReturnOkWithItems()
    {
        // Arrange
        var wishlistItems = new List<CartItem>
        {
            new CartItem { Id = 1, UserId = 1, BookId = 1, IsWishlist = true, Book = new Book { Id = 1, BookName = "Book 1", Price = 100 } },
            new CartItem { Id = 2, UserId = 1, BookId = 2, IsWishlist = true, Book = new Book { Id = 2, BookName = "Book 2", Price = 150 } }
        };

        _mockCartRepository
            .Setup(x => x.GetUserWishlistItemsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(wishlistItems);

        // Act
        var result = await _controller.GetWishlistItems();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetWishlistItems_WithEmptyWishlist_ShouldReturnOkWithEmptyList()
    {
        // Arrange
        _mockCartRepository
            .Setup(x => x.GetUserWishlistItemsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CartItem>());

        // Act
        var result = await _controller.GetWishlistItems();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetWishlistItems_WithoutAuth_ShouldReturnUnauthorized()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        // Act
        var result = await _controller.GetWishlistItems();

        // Assert
        Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
    }

    #endregion
}
