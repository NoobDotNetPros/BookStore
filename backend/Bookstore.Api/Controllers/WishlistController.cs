using Bookstore.Application.Contracts.Repositories;
using Bookstore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers;

[ApiController]
[Route("bookstore_user")]
[Tags("WishList")]
public class WishlistController : ControllerBase
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public WishlistController(ICartRepository cartRepository, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// WishList item to add by product_id
    /// </summary>
    [HttpPost("add_wish_list/{product_id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddToWishlist(int product_id)
    {
        // TODO: Get userId from JWT token
        int userId = 1;

        var wishlistItem = new CartItem
        {
            UserId = userId,
            BookId = product_id,
            Quantity = 1,
            IsWishlist = true
        };

        await _cartRepository.AddAsync(wishlistItem);
        await _unitOfWork.SaveChangesAsync();

        return Ok(new { message = "Successfully added product to wish list" });
    }

    /// <summary>
    /// Cart item to remove by cartItem_id
    /// </summary>
    [HttpDelete("remove_wishlist_item/{product_id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveFromWishlist(int product_id)
    {
        // TODO: Get userId from JWT token and find exact item
        await _cartRepository.DeleteAsync(product_id);
        await _unitOfWork.SaveChangesAsync();

        return Ok(new { message = "Successfully removed product from wish list" });
    }

    /// <summary>
    /// Get all wish list items
    /// </summary>
    [HttpGet("get_wishlist_items")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWishlistItems()
    {
        // TODO: Get userId from JWT token
        int userId = 1;

        var wishlistItems = await _cartRepository.GetUserWishlistItemsAsync(userId);
        return Ok(new { message = "Successfully fetched all wish list items", data = wishlistItems });
    }
}
