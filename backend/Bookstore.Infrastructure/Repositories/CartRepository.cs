using Bookstore.Application.Contracts.Repositories;
using Bookstore.Domain.Entities;
using Bookstore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _context;

    public CartRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CartItem>> GetUserCartItemsAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.CartItems
            .Include(c => c.Book)
            .Where(c => c.UserId == userId && !c.IsWishlist)
            .ToListAsync(cancellationToken);
    }

    public async Task<CartItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.CartItems
            .Include(c => c.Book)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<CartItem> AddAsync(CartItem cartItem, CancellationToken cancellationToken = default)
    {
        await _context.CartItems.AddAsync(cartItem, cancellationToken);
        return cartItem;
    }

    public Task UpdateAsync(CartItem cartItem, CancellationToken cancellationToken = default)
    {
        _context.CartItems.Update(cartItem);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var cartItem = await _context.CartItems.FindAsync(new object[] { id }, cancellationToken);
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
        }
    }
}
