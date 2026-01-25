using FluentValidation;

namespace Bookstore.Application.Features.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.BookName)
            .NotEmpty().WithMessage("Book name is required.")
            .MaximumLength(200).WithMessage("Book name must not exceed 200 characters.");

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("Author is required.")
            .MaximumLength(100).WithMessage("Author must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity must be zero or greater.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.DiscountPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Discount price must be zero or greater.")
            .LessThanOrEqualTo(x => x.Price).WithMessage("Discount price cannot exceed price.");
    }
}
