using Bookstore.Domain.Entities;

namespace Bookstore.Application.Contracts.Repositories;

public interface ICartRepository
{
    Task<List<CartItem>> GetUserCartItemsAsync(int userId, CancellationToken cancellationToken = default);
    Task<List<CartItem>> GetUserWishlistItemsAsync(int userId, CancellationToken cancellationToken = default); // ADD THIS
    Task<CartItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CartItem?> GetUserCartItemByBookIdAsync(int userId, int bookId, CancellationToken cancellationToken = default); // ADD THIS
    Task<CartItem> AddAsync(CartItem cartItem, CancellationToken cancellationToken = default);
    Task UpdateAsync(CartItem cartItem, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
