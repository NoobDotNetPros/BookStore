using MediatR;

namespace Bookstore.Business.Services.Books.Commands;

public record DeleteBookCommand(int Id) : IRequest<Unit>;
