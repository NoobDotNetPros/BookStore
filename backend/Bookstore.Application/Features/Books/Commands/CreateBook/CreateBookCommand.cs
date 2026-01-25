using MediatR;

namespace Bookstore.Application.Features.Books.Commands.CreateBook;

public record CreateBookCommand(
    string BookName,
    string Author,
    string Description,
    int Quantity,
    decimal Price,
    decimal DiscountPrice
) : IRequest<int>;
