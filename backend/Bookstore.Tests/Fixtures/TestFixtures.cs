using Moq;
using Bookstore.Business.Interfaces;
using Bookstore.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Tests.Fixtures;

public class TestFixtures
{
    /// <summary>
    /// Creates an in-memory database context for testing
    /// </summary>
    public static ApplicationDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    /// <summary>
    /// Creates mock repositories for testing
    /// </summary>
    public static Mock<IBookRepository> CreateMockBookRepository()
    {
        return new Mock<IBookRepository>();
    }

    public static Mock<IUserRepository> CreateMockUserRepository()
    {
        return new Mock<IUserRepository>();
    }

    public static Mock<ICartRepository> CreateMockCartRepository()
    {
        return new Mock<ICartRepository>();
    }

    public static Mock<IOrderRepository> CreateMockOrderRepository()
    {
        return new Mock<IOrderRepository>();
    }

    public static Mock<IUnitOfWork> CreateMockUnitOfWork()
    {
        return new Mock<IUnitOfWork>();
    }

    public static Mock<IEmailService> CreateMockEmailService()
    {
        return new Mock<IEmailService>();
    }
}
