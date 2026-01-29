using Bookstore.Business.Interfaces;
using Bookstore.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Bookstore.DataAccess.Context;

namespace Bookstore.Web.Controllers;

[ApiController]
[Route("api/orders")]
[Tags("Order")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderController(
        IOrderRepository orderRepository, 
        IUserRepository userRepository,
        ICartRepository cartRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            throw new UnauthorizedAccessException("Invalid user token");
        }
        return userId;
    }

    /// <summary>
    /// Add new order
    /// </summary>
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var userId = GetUserId();

        // Get user's address for shipping
        var user = await _userRepository.GetByIdAsync(userId);
        var shippingAddress = user?.Addresses?.FirstOrDefault();
        
        string addressString = shippingAddress != null 
            ? $"{shippingAddress.FullAddress}, {shippingAddress.City}, {shippingAddress.State}" 
            : "No address on file";

        decimal totalAmount = request.Orders.Sum(o => o.Product_Price * o.Product_Quantity);

        var order = new Order
        {
            UserId = userId,
            Status = OrderStatus.Pending,
            TotalAmount = totalAmount,
            ShippingAddress = addressString,
            OrderItems = request.Orders.Select(o => new OrderItem
            {
                BookId = int.Parse(o.Product_Id),
                Quantity = o.Product_Quantity,
                Price = o.Product_Price
            }).ToList()
        };

        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _orderRepository.AddAsync(order);
            
            // Clear the cart after successful order
            await _cartRepository.ClearCartAsync(userId);
            
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

        return Ok(new { success = true, message = "Order created successfully", data = new { orderId = order.Id } });
    }

    /// <summary>
    /// Get all user orders
    /// </summary>
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserOrders()
    {
        var userId = GetUserId();

        var orders = await _orderRepository.GetUserOrdersAsync(userId);
        return Ok(new { success = true, message = "Successfully fetched user orders", data = orders });
    }

    /// <summary>
    /// Get order by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var userId = GetUserId();
        
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            return NotFound(new { success = false, message = "Order not found" });

        // Verify the order belongs to the user
        if (order.UserId != userId)
            return NotFound(new { success = false, message = "Order not found" });

        return Ok(new { success = true, message = "Successfully fetched order", data = order });
    }
}

public record CreateOrderRequest(List<OrderItemRequest> Orders);

public record OrderItemRequest(
    string Product_Id,
    string Product_Name,
    int Product_Quantity,
    decimal Product_Price
);
