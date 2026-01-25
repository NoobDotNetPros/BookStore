using Bookstore.Application.Features.Books.Dtos;
using MediatR;

namespace Bookstore.Application.Features.Books.Queries.GetBooks;

public record GetBooksQuery() : IRequest<List<BookDto>>;
