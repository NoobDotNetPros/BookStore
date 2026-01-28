using Bookstore.Business.Interfaces;
using Bookstore.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Controllers;

[ApiController]
[Route("api/cart")]
[Tags("Cart")]
public class CartController : ControllerBase
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CartController(ICartRepository cartRepository, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Cart item to add by product_id
    /// </summary>
    [HttpPost("items")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddToCart([FromBody] AddCartItemRequest request)
    {
        // TODO: Get userId from JWT token
        int userId = 1;

        var cartItem = new CartItem
        {
            UserId = userId,
            BookId = request.BookId,
            Quantity = request.Quantity,
            IsWishlist = false
        };

        await _cartRepository.AddAsync(cartItem);
        await _unitOfWork.SaveChangesAsync();

        return Ok(new { message = "Successfully added product to cart" });
    }

    /// <summary>
    /// Update cart item quantity by cartItem_id
    /// </summary>
    [HttpPut("items/{cartItem_id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateQuantity(int cartItem_id, [FromBody] UpdateQuantityRequest request)
    {
        var cartItem = await _cartRepository.GetByIdAsync(cartItem_id);
        if (cartItem == null)
            return NotFound(new { message = "Cart item not found" });

        cartItem.Quantity = request.QuantityToBuy;
        await _cartRepository.UpdateAsync(cartItem);
        await _unitOfWork.SaveChangesAsync();

        return Ok(new { message = "Successfully updated cart item quantity" });
    }

    /// <summary>
    /// Cart item to remove by cartItem_id
    /// </summary>
    [HttpDelete("items/{cartItem_id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveFromCart(int cartItem_id)
    {
        await _cartRepository.DeleteAsync(cartItem_id);
        await _unitOfWork.SaveChangesAsync();

        return Ok(new { message = "Successfully removed product from cart" });
    }

    /// <summary>
    /// Get all cart items
    /// </summary>
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCartItems()
    {
        // TODO: Get userId from JWT token
        int userId = 1;

        var cartItems = await _cartRepository.GetUserCartItemsAsync(userId);
        return Ok(new { message = "Successfully fetched all cart items", data = cartItems });
    }
}

public record UpdateQuantityRequest(int QuantityToBuy);
public record AddCartItemRequest(int BookId, int Quantity);
