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
                    Description = "A Handbook of Agile Software Craftsmanship - Learn how to write code that is readable, maintainable, and elegant.",
                    ISBN = "9780132350884",
                    Quantity = 50,
                    Price = 1200,
                    DiscountPrice = 900,
                    CoverImage = "https://covers.openlibrary.org/b/id/7224456-M.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Book
                {
                    BookName = "The Pragmatic Programmer",
                    Author = "Andrew Hunt & David Thomas",
                    Description = "Your Journey to Mastery - A practical guide to becoming a better programmer through pragmatic thinking.",
                    ISBN = "9780135957059",
                    Quantity = 30,
                    Price = 1500,
                    DiscountPrice = 1200,
                    CoverImage = "https://covers.openlibrary.org/b/id/8370439-M.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Book
                {
                    BookName = "Design Patterns",
                    Author = "Gang of Four",
                    Description = "Elements of Reusable Object-Oriented Software - Comprehensive guide to design patterns and software design principles.",
                    ISBN = "9780201633610",
                    Quantity = 25,
                    Price = 2000,
                    DiscountPrice = 1500,
                    CoverImage = "https://covers.openlibrary.org/b/id/7174269-M.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Book
                {
                    BookName = "Introduction to Algorithms",
                    Author = "Cormen, Leiserson, Rivest & Stein",
                    Description = "MIT Press classic text on algorithm design and analysis - Essential for computer science students.",
                    ISBN = "9780262033848",
                    Quantity = 40,
                    Price = 2500,
                    DiscountPrice = 1900,
                    CoverImage = "https://covers.openlibrary.org/b/id/7893813-M.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Book
                {
                    BookName = "The C Programming Language",
                    Author = "Brian W. Kernighan & Dennis M. Ritchie",
                    Description = "The definitive guide to C programming - Perfect for learning the fundamentals of systems programming.",
                    ISBN = "9780131103627",
                    Quantity = 35,
                    Price = 1100,
                    DiscountPrice = 800,
                    CoverImage = "https://covers.openlibrary.org/b/id/7170706-M.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Book
                {
                    BookName = "Refactoring: Improving the Design of Existing Code",
                    Author = "Martin Fowler",
                    Description = "Learn practical techniques to improve code quality and maintainability without breaking functionality.",
                    ISBN = "9780201485677",
                    Quantity = 28,
                    Price = 1400,
                    DiscountPrice = 1000,
                    CoverImage = "https://covers.openlibrary.org/b/id/8386938-M.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Book
                {
                    BookName = "Code Complete",
                    Author = "Steve McConnell",
                    Description = "A Practical Handbook of Software Construction - Comprehensive guide to professional software development.",
                    ISBN = "9780735619678",
                    Quantity = 22,
                    Price = 1800,
                    DiscountPrice = 1300,
                    CoverImage = "https://covers.openlibrary.org/b/id/7833022-M.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Book
                {
                    BookName = "Effective Java",
                    Author = "Joshua Bloch",
                    Description = "Programming Language Guide - Learn 78 essential programmer practices for writing better Java code.",
                    ISBN = "9780134685991",
                    Quantity = 45,
                    Price = 1300,
                    DiscountPrice = 950,
                    CoverImage = "https://covers.openlibrary.org/b/id/12030146-M.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Book
                {
                    BookName = "The Mythical Man-Month",
                    Author = "Frederick P. Brooks Jr.",
                    Description = "Essays on Software Engineering - Timeless insights into software project management and team dynamics.",
                    ISBN = "9780201633610",
                    Quantity = 18,
                    Price = 1250,
                    DiscountPrice = 900,
                    CoverImage = "https://covers.openlibrary.org/b/id/7893819-M.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Book
                {
                    BookName = "Head First Design Patterns",
                    Author = "Freeman & Robson",
                    Description = "Building Extensible and Maintainable Object-Oriented Software - Learn design patterns through engaging visuals.",
                    ISBN = "9780596007126",
                    Quantity = 32,
                    Price = 1600,
                    DiscountPrice = 1200,
                    CoverImage = "https://covers.openlibrary.org/b/id/8369952-M.jpg",
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Books.AddRange(books);
            await context.SaveChangesAsync();
        }
    }
}
