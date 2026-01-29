using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Web.Controllers;
using Bookstore.Business.Interfaces;
using Bookstore.Models.Entities;
using Bookstore.Models.DTOs;
using Bookstore.Models;

namespace Bookstore.Tests.Controllers;

[TestFixture]
public class AdminControllerTests
{
    private AdminController _controller = null!;
    private Mock<IBookRepository> _mockBookRepository = null!;
    private Mock<IUnitOfWork> _mockUnitOfWork = null!;

    [SetUp]
    public void Setup()
    {
        _mockBookRepository = new Mock<IBookRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _controller = new AdminController(
            _mockBookRepository.Object,
            _mockUnitOfWork.Object);
    }

    #region GetAllBooks Tests

    [Test]
    public async Task GetAllBooks_ShouldReturnOkWithBooks()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book { Id = 1, BookName = "Book 1", Author = "Author 1", Price = 100 },
            new Book { Id = 2, BookName = "Book 2", Author = "Author 2", Price = 150 }
        };

        _mockBookRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(books);

        // Act
        var result = await _controller.GetAllBooks();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var response = okResult!.Value as ApiResponse<List<BookDto>>;
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Success, Is.True);
    }

    [Test]
    public async Task GetAllBooks_WithEmptyList_ShouldReturnOkWithEmptyList()
    {
        // Arrange
        _mockBookRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Book>());

        // Act
        var result = await _controller.GetAllBooks();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
    }

    #endregion

    #region GetBookById Tests

    [Test]
    public async Task GetBookById_WithValidId_ShouldReturnOkWithBook()
    {
        // Arrange
        var book = new Book { Id = 1, BookName = "Test Book", Author = "Author", Price = 100 };

        _mockBookRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        // Act
        var result = await _controller.GetBookById(1);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
    }

    [Test]
    public async Task GetBookById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        _mockBookRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        // Act
        var result = await _controller.GetBookById(999);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
    }

    #endregion

    #region CreateBook Tests

    [Test]
    public async Task CreateBook_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var createBookDto = new CreateBookDto
        {
            BookName = "New Book",
            Author = "New Author",
            Description = "Description",
            ISBN = "123-456",
            Quantity = 10,
            Price = 100,
            DiscountPrice = 80
        };

        _mockBookRepository
            .Setup(x => x.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Book { Id = 1 });

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.CreateBook(createBookDto);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
    }

    [Test]
    public async Task CreateBook_ShouldCallRepositoryAddAsync()
    {
        // Arrange
        var createBookDto = new CreateBookDto
        {
            BookName = "New Book",
            Author = "New Author",
            Description = "Description",
            ISBN = "123-456",
            Quantity = 10,
            Price = 100,
            DiscountPrice = 80
        };

        _mockBookRepository
            .Setup(x => x.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Book { Id = 1 });

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _controller.CreateBook(createBookDto);

        // Assert
        _mockBookRepository.Verify(x => x.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region UpdateBook Tests

    [Test]
    public async Task UpdateBook_WithValidData_ShouldReturnOk()
    {
        // Arrange
        var updateBookDto = new UpdateBookDto
        {
            Id = 1,
            BookName = "Updated Book",
            Author = "Updated Author",
            Description = "Updated Description",
            ISBN = "123-456",
            Quantity = 15,
            Price = 120,
            DiscountPrice = 100
        };

        var existingBook = new Book { Id = 1, BookName = "Old Book", Author = "Old Author" };

        _mockBookRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBook);

        _mockBookRepository
            .Setup(x => x.UpdateAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.UpdateBook(1, updateBookDto);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task UpdateBook_WithNonExistentBook_ShouldReturnNotFound()
    {
        // Arrange
        var updateBookDto = new UpdateBookDto
        {
            Id = 999,
            BookName = "Updated Book",
            Author = "Updated Author"
        };

        _mockBookRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        // Act
        var result = await _controller.UpdateBook(999, updateBookDto);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task UpdateBook_WithMismatchedId_ShouldReturnBadRequest()
    {
        // Arrange
        var updateBookDto = new UpdateBookDto
        {
            Id = 2, // Different from the URL id
            BookName = "Updated Book",
            Author = "Updated Author"
        };

        // Act
        var result = await _controller.UpdateBook(1, updateBookDto);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
    }

    #endregion

    #region DeleteBook Tests

    [Test]
    public async Task DeleteBook_WithValidId_ShouldReturnOk()
    {
        // Arrange
        var book = new Book { Id = 1, BookName = "Test Book", Author = "Author" };

        _mockBookRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        _mockBookRepository
            .Setup(x => x.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.DeleteBook(1);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task DeleteBook_WithNonExistentBook_ShouldReturnNotFound()
    {
        // Arrange
        _mockBookRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        // Act
        var result = await _controller.DeleteBook(999);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
    }

    #endregion
}
