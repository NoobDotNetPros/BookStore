using Bookstore.Business.Interfaces;
using Bookstore.Business.Models;
using Bookstore.Models.Entities;
using MediatR;

namespace Bookstore.Business.Services.Books.Commands;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Unit>
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Id, cancellationToken);

        if (book == null)
            throw new NotFoundException(nameof(Book), request.Id);

        book.BookName = request.BookName;
        book.Author = request.Author;
        book.Description = request.Description;
        book.Quantity = request.Quantity;
        book.Price = request.Price;
        book.DiscountPrice = request.DiscountPrice;
        book.UpdatedAt = DateTime.UtcNow;

        await _bookRepository.UpdateAsync(book, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
