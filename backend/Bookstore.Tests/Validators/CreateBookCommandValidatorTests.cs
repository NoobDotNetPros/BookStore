using Bookstore.Business.Services.Books.Commands;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Bookstore.Tests.Validators;

[TestFixture]
public class CreateBookCommandValidatorTests
{
    private CreateBookCommandValidator _validator = null!;

    [SetUp]
    public void Setup()
    {
        _validator = new CreateBookCommandValidator();
    }

    [Test]
    public void Validate_WithValidCommand_ShouldNotHaveErrors()
    {
        var command = new CreateBookCommand(
            BookName: "Test Book",
            Author: "Test Author",
            Description: "Test Description",
            Quantity: 10,
            Price: 29.99m,
            DiscountPrice: 19.99m
        );

        var result = _validator.TestValidate(command);
        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void Validate_WithEmptyBookName_ShouldHaveError()
    {
        var command = new CreateBookCommand("", "Author", "Description", 1, 10m, 5m);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.BookName);
    }

    [Test]
    public void Validate_WithEmptyAuthor_ShouldHaveError()
    {
        var command = new CreateBookCommand("Book", "", "Description", 1, 10m, 5m);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Author);
    }

    [Test]
    public void Validate_WithNegativeQuantity_ShouldHaveError()
    {
        var command = new CreateBookCommand("Book", "Author", "Description", -1, 10m, 5m);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Test]
    public void Validate_WithZeroPrice_ShouldHaveError()
    {
        var command = new CreateBookCommand("Book", "Author", "Description", 1, 0, 0);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Test]
    public void Validate_WithDiscountPriceGreaterThanPrice_ShouldHaveError()
    {
        var command = new CreateBookCommand("Book", "Author", "Description", 1, 10.00m, 15.00m);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.DiscountPrice);
    }
}
