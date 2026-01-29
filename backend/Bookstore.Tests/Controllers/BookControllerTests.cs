using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Bookstore.Web.Controllers;
using Bookstore.Business.Services.Books.Queries;
using Bookstore.Models.DTOs;

namespace Bookstore.Tests.Controllers;

[TestFixture]
public class BookControllerTests
{
    private BookController _controller = null!;
    private Mock<IMediator> _mockMediator = null!;

    [SetUp]
    public void Setup()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new BookController(_mockMediator.Object);
    }

    #region GetAllBooks Tests

    [Test]
    public async Task GetAllBooks_ShouldReturnOkWithBooks()
    {
        // Arrange
        var books = new List<BookDto>
        {
            new BookDto { Id = 1, BookName = "Book 1", Author = "Author 1", Price = 100 },
            new BookDto { Id = 2, BookName = "Book 2", Author = "Author 2", Price = 150 }
        };

        _mockMediator
            .Setup(x => x.Send(It.IsAny<GetBooksQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(books);

        // Act
        var result = await _controller.GetAllBooks();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
    }

    [Test]
    public async Task GetAllBooks_WithEmptyList_ShouldReturnOkWithEmptyList()
    {
        // Arrange
        var books = new List<BookDto>();

        _mockMediator
            .Setup(x => x.Send(It.IsAny<GetBooksQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(books);

        // Act
        var result = await _controller.GetAllBooks();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    #endregion

    #region GetBookById Tests

    [Test]
    public async Task GetBookById_WithValidId_ShouldReturnOkWithBook()
    {
        // Arrange
        var book = new BookDto { Id = 1, BookName = "Test Book", Author = "Author", Price = 100 };

        _mockMediator
            .Setup(x => x.Send(It.IsAny<GetBookByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        // Act
        var result = await _controller.GetBookById(1);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
    }

    [Test]
    public async Task GetBookById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        _mockMediator
            .Setup(x => x.Send(It.IsAny<GetBookByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((BookDto?)null);

        // Act
        var result = await _controller.GetBookById(999);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    #endregion
}
