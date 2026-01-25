using Bookstore.Domain.Entities; 

namespace Bookstore.Application.Contracts.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default);
        Task UpdateAsync(Book book, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
