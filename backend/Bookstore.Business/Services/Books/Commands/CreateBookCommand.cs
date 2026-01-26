using MediatR;

namespace Bookstore.Business.Services.Books.Commands;

public record CreateBookCommand(
    string BookName,
    string Author,
    string Description,
    int Quantity,
    decimal Price,
    decimal DiscountPrice
) : IRequest<int>;
