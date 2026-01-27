using MediatR;

namespace Bookstore.Business.Services.Books.Commands;

public record UpdateBookCommand(
    int Id,
    string BookName,
    string Author,
    string Description,
    int Quantity,
    decimal Price,
    decimal DiscountPrice
) : IRequest<Unit>;
