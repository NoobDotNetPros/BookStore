using Bookstore.Models.DTOs;
using MediatR;

namespace Bookstore.Business.Services.Books.Queries;

public record GetBookByIdQuery(int Id) : IRequest<BookDto?>;
