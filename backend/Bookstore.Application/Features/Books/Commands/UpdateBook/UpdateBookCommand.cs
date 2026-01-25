using MediatR;

namespace Bookstore.Application.Features.Books.Commands.UpdateBook;

public record UpdateBookCommand(
    int Id,
    string BookName,
    string Author,
    string Description,
    int Quantity,
    decimal Price,
    decimal DiscountPrice
) : IRequest<Unit>;
