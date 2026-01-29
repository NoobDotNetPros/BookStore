using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bookstore.Web.Controllers;
using Bookstore.DataAccess.Context;
using Bookstore.Models.Entities;

namespace Bookstore.Tests.Controllers;

[TestFixture]
public class FeedbackControllerTests
{
    private FeedbackController _controller = null!;
    private ApplicationDbContext _context = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _controller = new FeedbackController(_context);

        // Seed test data
        SeedTestData();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private void SeedTestData()
    {
        var user = new User
        {
            Id = 1,
            FullName = "Test User",
            Email = "test@test.com",
            PasswordHash = "hash",
            Phone = "1234567890"
        };

        var book = new Book
        {
            Id = 1,
            BookName = "Test Book",
            Author = "Test Author",
            Description = "Test Description",
            ISBN = "123-456-789",
            Quantity = 10,
            Price = 100,
            DiscountPrice = 80
        };

        _context.Users.Add(user);
        _context.Books.Add(book);
        _context.SaveChanges();
    }

    #region AddFeedback Tests

    [Test]
    public async Task AddFeedback_WithValidRequest_ShouldReturnOk()
    {
        // Arrange
        int productId = 1;
        var request = new FeedbackRequest(Comment: "Great book!", Rating: "5");

        // Act
        var result = await _controller.AddFeedback(productId, request);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());

        // Verify feedback was added
        var feedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.BookId == productId);
        Assert.That(feedback, Is.Not.Null);
        Assert.That(feedback!.Comment, Is.EqualTo("Great book!"));
        Assert.That(feedback.Rating, Is.EqualTo(5));
    }

    [Test]
    public async Task AddFeedback_MultipleFeedbacks_ShouldAddAll()
    {
        // Arrange
        int productId = 1;
        var request1 = new FeedbackRequest(Comment: "Amazing book!", Rating: "5");
        var request2 = new FeedbackRequest(Comment: "Good read", Rating: "4");

        // Act
        await _controller.AddFeedback(productId, request1);
        await _controller.AddFeedback(productId, request2);

        // Assert
        var feedbacks = await _context.Feedbacks.Where(f => f.BookId == productId).ToListAsync();
        Assert.That(feedbacks.Count, Is.EqualTo(2));
    }

    #endregion

    #region GetFeedbacks Tests

    [Test]
    public async Task GetFeedbacks_WithExistingFeedbacks_ShouldReturnOkWithFeedbacks()
    {
        // Arrange
        int productId = 1;
        _context.Feedbacks.Add(new Feedback
        {
            UserId = 1,
            BookId = productId,
            Comment = "Great book!",
            Rating = 5
        });
        _context.Feedbacks.Add(new Feedback
        {
            UserId = 1,
            BookId = productId,
            Comment = "Good read",
            Rating = 4
        });
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetFeedbacks(productId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetFeedbacks_WithNoFeedbacks_ShouldReturnOkWithEmptyList()
    {
        // Arrange
        int productId = 1;

        // Act
        var result = await _controller.GetFeedbacks(productId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetFeedbacks_WithNonExistentProduct_ShouldReturnOkWithEmptyList()
    {
        // Arrange
        int productId = 999;

        // Act
        var result = await _controller.GetFeedbacks(productId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    #endregion
}
