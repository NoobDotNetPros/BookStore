using Bookstore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.DataAccess.Context;

public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context)
    {
        // Apply pending migrations
        await context.Database.MigrateAsync();

        // Seed admin user if no users exist
        if (!await context.Users.AnyAsync())
        {
            var admin = new User
            {
                FullName = "Bookstore Admin",
                Email = "admin@bookstore.com",
                PasswordHash = "Admin@123", // TODO: Hash this in production
                Phone = "9999999999",
                Role = UserRole.Admin,
                IsEmailVerified = true,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }

        // Seed sample books
        if (!await context.Books.AnyAsync())
        {
            var books = new List<Book>
            {
                new Book
                {
                    BookName = "Clean Code",
                    Author = "Robert C. Martin",
                    Description = "A Handbook of Agile Software Craftsmanship",
                    ISBN = "9780132350884",
                    Quantity = 50,
                    Price = 1200,
                    DiscountPrice = 900,
                    CreatedAt = DateTime.UtcNow
                },
                new Book
                {
                    BookName = "The Pragmatic Programmer",
                    Author = "Andrew Hunt",
                    Description = "Your Journey To Mastery",
                    ISBN = "9780135957059",
                    Quantity = 30,
                    Price = 1500,
                    DiscountPrice = 1200,
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Books.AddRange(books);
            await context.SaveChangesAsync();
        }
    }
}
