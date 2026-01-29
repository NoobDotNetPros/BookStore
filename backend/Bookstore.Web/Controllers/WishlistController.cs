using Bookstore.Business.Interfaces;
using Bookstore.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Controllers;

[ApiController]
[Route("api/wishlist")]
[Tags("WishList")]
[Authorize]
public class WishlistController : ControllerBase
{
    private readonly ICartRepository _cartRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public WishlistController(ICartRepository cartRepository, IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    private int? GetUserId()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) 
            ?? User.FindFirst("sub");
        
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            return userId;
        
        return null;
    }

    /// <summary>
    /// WishList item to add by product_id
    /// </summary>
    [HttpPost("items/{product_id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddToWishlist(int product_id)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized(new { success = false, message = "Invalid or missing user token" });

        // Check if book exists
        var book = await _bookRepository.GetByIdAsync(product_id);
        if (book == null)
            return BadRequest(new { success = false, message = "Book not found" });

        // Check if item already exists in wishlist
        var existingItems = await _cartRepository.GetUserWishlistItemsAsync(userId.Value);
        if (existingItems.Any(i => i.BookId == product_id))
        {
            return BadRequest(new { success = false, message = "Item already in wishlist" });
        }

        var wishlistItem = new CartItem
        {
            UserId = userId.Value,
            BookId = product_id,
            Quantity = 1,
            IsWishlist = true
        };

        await _cartRepository.AddAsync(wishlistItem);
        await _unitOfWork.SaveChangesAsync();

        // Return item details
        var itemResponse = new
        {
            id = wishlistItem.Id,
            bookId = book.Id,
            bookTitle = book.BookName,
            coverImage = book.CoverImage ?? "",
            price = book.DiscountPrice,
            originalPrice = book.Price
        };

        return Ok(new { success = true, message = "Successfully added product to wish list", data = itemResponse });
    }

    /// <summary>
    /// Cart item to remove by product_id (bookId)
    /// </summary>
    [HttpDelete("items/{product_id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveFromWishlist(int product_id)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized(new { success = false, message = "Invalid or missing user token" });

        // Find the wishlist item by bookId
        var wishlistItems = await _cartRepository.GetUserWishlistItemsAsync(userId.Value);
        var itemToRemove = wishlistItems.FirstOrDefault(i => i.BookId == product_id);

        if (itemToRemove == null)
            return NotFound(new { success = false, message = "Item not found in wishlist" });

        await _cartRepository.DeleteAsync(itemToRemove.Id);
        await _unitOfWork.SaveChangesAsync();

        return Ok(new { success = true, message = "Successfully removed product from wish list" });
    }

    /// <summary>
    /// Get all wish list items
    /// </summary>
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetWishlistItems()
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized(new { success = false, message = "Invalid or missing user token" });

        var wishlistItems = await _cartRepository.GetUserWishlistItemsAsync(userId.Value);

        var items = wishlistItems.Select(item => new
        {
            id = item.Id,
            bookId = item.BookId,
            bookTitle = item.Book?.BookName ?? "Unknown",
            author = item.Book?.Author ?? "Unknown",
            coverImage = item.Book?.CoverImage ?? "",
            price = item.Book?.DiscountPrice ?? 0,
            originalPrice = item.Book?.Price ?? 0
        }).ToList();

        return Ok(new { success = true, message = "Successfully fetched all wish list items", data = items });
    }
}
