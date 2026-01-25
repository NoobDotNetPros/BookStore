using Bookstore.Application.Contracts.Repositories;
using Bookstore.Domain.Entities;
using MediatR;

namespace Bookstore.Application.Features.Books.Commands.CreateBook;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, int>
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book
        {
            BookName = request.BookName,
            Author = request.Author,
            Description = request.Description,
            Quantity = request.Quantity,
            Price = request.Price,
            DiscountPrice = request.DiscountPrice
        };

        await _bookRepository.AddAsync(book, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return book.Id;
    }
}
