using Bookstore.Models.DTOs;
using MediatR;

namespace Bookstore.Business.Services.Books.Queries;

public record GetBooksQuery() : IRequest<List<BookDto>>;
