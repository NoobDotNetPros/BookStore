using MediatR;

namespace Bookstore.Application.Features.Books.Commands.DeleteBook;

public record DeleteBookCommand(int Id) : IRequest<Unit>;
