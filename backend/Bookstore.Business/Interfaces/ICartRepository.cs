using Bookstore.Models.Entities;

namespace Bookstore.Business.Interfaces;

public interface ICartRepository
{
    Task<List<CartItem>> GetUserCartItemsAsync(int userId, CancellationToken cancellationToken = default);
    Task<List<CartItem>> GetUserWishlistItemsAsync(int userId, CancellationToken cancellationToken = default);
    Task<CartItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CartItem?> GetUserCartItemByBookIdAsync(int userId, int bookId, CancellationToken cancellationToken = default);
    Task<CartItem> AddAsync(CartItem cartItem, CancellationToken cancellationToken = default);
    Task UpdateAsync(CartItem cartItem, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
