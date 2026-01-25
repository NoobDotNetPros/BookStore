using Bookstore.Domain.Entities;

namespace Bookstore.Application.Contracts.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<Order>> GetUserOrdersAsync(int userId, CancellationToken cancellationToken = default);
        Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default);
    }
}
