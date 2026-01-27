using AutoMapper;
using Bookstore.Business.Interfaces;
using Bookstore.Models.DTOs;
using MediatR;

namespace Bookstore.Business.Services.Books.Queries;

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, List<BookDto>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public GetBooksQueryHandler(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<List<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<BookDto>>(books);
    }
}
