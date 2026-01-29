using Bookstore.Business.Interfaces;
using Bookstore.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Controllers;

[ApiController]
[Route("api/cart")]
[Tags("Cart")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartRepository _cartRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CartController(ICartRepository cartRepository, IBookRepository bookRepository, IUnitOfWork unitOfWork)
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
    /// Cart item to add by product_id
    /// </summary>
    [HttpPost("items")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddToCart([FromBody] AddCartItemRequest request)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized(new { success = false, message = "Invalid or missing user token" });

        // Check if book exists
        var book = await _bookRepository.GetByIdAsync(request.BookId);
        if (book == null)
            return BadRequest(new { success = false, message = "Book not found" });

        // Check if item already exists in cart
        var existingItem = await _cartRepository.GetUserCartItemByBookIdAsync(userId.Value, request.BookId);
        if (existingItem != null && !existingItem.IsWishlist)
        {
            // Update quantity instead
            existingItem.Quantity += request.Quantity;
            await _cartRepository.UpdateAsync(existingItem);
        }
        else
        {
            var cartItem = new CartItem
            {
                UserId = userId.Value,
                BookId = request.BookId,
                Quantity = request.Quantity,
                IsWishlist = false
            };
            await _cartRepository.AddAsync(cartItem);
        }

        await _unitOfWork.SaveChangesAsync();

        // Return updated cart
        var cartItems = await _cartRepository.GetUserCartItemsAsync(userId.Value);
        var cartResponse = MapCartResponse(cartItems, userId.Value);

        return Ok(new { success = true, message = "Successfully added product to cart", data = cartResponse });
    }

    /// <summary>
    /// Update cart item quantity by cartItem_id
    /// </summary>
    [HttpPut("items/{cartItem_id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateQuantity(int cartItem_id, [FromBody] UpdateQuantityRequest request)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized(new { success = false, message = "Invalid or missing user token" });

        var cartItem = await _cartRepository.GetByIdAsync(cartItem_id);
        if (cartItem == null || cartItem.UserId != userId.Value)
            return NotFound(new { success = false, message = "Cart item not found" });

        cartItem.Quantity = request.Quantity > 0 ? request.Quantity : request.QuantityToBuy;
        await _cartRepository.UpdateAsync(cartItem);
        await _unitOfWork.SaveChangesAsync();

        // Return updated cart
        var cartItems = await _cartRepository.GetUserCartItemsAsync(userId.Value);
        var cartResponse = MapCartResponse(cartItems, userId.Value);

        return Ok(new { success = true, message = "Successfully updated cart item quantity", data = cartResponse });
    }

    /// <summary>
    /// Cart item to remove by cartItem_id
    /// </summary>
    [HttpDelete("items/{cartItem_id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveFromCart(int cartItem_id)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized(new { success = false, message = "Invalid or missing user token" });

        var cartItem = await _cartRepository.GetByIdAsync(cartItem_id);
        if (cartItem == null || cartItem.UserId != userId.Value)
            return NotFound(new { success = false, message = "Cart item not found" });

        await _cartRepository.DeleteAsync(cartItem_id);
        await _unitOfWork.SaveChangesAsync();

        // Return updated cart
        var cartItems = await _cartRepository.GetUserCartItemsAsync(userId.Value);
        var cartResponse = MapCartResponse(cartItems, userId.Value);

        return Ok(new { success = true, message = "Successfully removed product from cart", data = cartResponse });
    }

    /// <summary>
    /// Get all cart items
    /// </summary>
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCartItems()
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized(new { success = false, message = "Invalid or missing user token" });

        var cartItems = await _cartRepository.GetUserCartItemsAsync(userId.Value);
        var cartResponse = MapCartResponse(cartItems, userId.Value);

        return Ok(new { success = true, message = "Successfully fetched all cart items", data = cartResponse });
    }

    private object MapCartResponse(List<CartItem> cartItems, int userId)
    {
        var items = cartItems.Select(item => new
        {
            id = item.Id,
            bookId = item.BookId,
            bookTitle = item.Book?.BookName ?? "Unknown",
            bookAuthor = item.Book?.Author ?? "Unknown",
            bookCoverImage = item.Book?.CoverImage ?? "",
            price = item.Book?.DiscountPrice ?? 0,
            originalPrice = item.Book?.Price ?? 0,
            quantity = item.Quantity
        }).ToList();

        return new
        {
            id = 0,
            userId = userId.ToString(),
            items = items,
            totalPrice = items.Sum(i => i.price * i.quantity)
        };
    }
}

public record UpdateQuantityRequest(int QuantityToBuy, int Quantity = 0);
public record AddCartItemRequest(int BookId, int Quantity);
