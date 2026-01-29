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
        if (request == null || request.Orders == null || !request.Orders.Any())
        {
            return BadRequest(new { success = false, message = "Invalid order request. Orders list cannot be empty." });
        }

        try
        {
            var userId = GetUserId();

            // Get user's address for shipping
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                Console.WriteLine($"[CreateOrder] User not found for ID: {userId}");
                return BadRequest(new { success = false, message = "User not found" });
            }

            var shippingAddress = user.Addresses?.FirstOrDefault();
            
            string addressString = shippingAddress != null 
                ? $"{shippingAddress.FullAddress}, {shippingAddress.City}, {shippingAddress.State}" 
                : "No address on file";

            var orderItems = new List<OrderItem>();
            foreach(var o in request.Orders) 
            {
                 // Handle potential null Product_Id from frontend
                 if (string.IsNullOrEmpty(o.Product_Id))
                 {
                     Console.WriteLine($"[CreateOrder] Missing Product_Id for item: {System.Text.Json.JsonSerializer.Serialize(o)}");
                     continue; // Skip invalid items or return BadRequest
                 }

                 if(int.TryParse(o.Product_Id, out int bookId))
                 {
                    orderItems.Add(new OrderItem
                    {
                        BookId = bookId,
                        Quantity = o.Product_Quantity,
                        Price = o.Product_Price
                    });
                 }
                 else
                 {
                     Console.WriteLine($"[CreateOrder] Invalid Book ID: {o.Product_Id}");
                     return BadRequest(new { success = false, message = $"Invalid Book ID format: {o.Product_Id}" });
                 }
            }

            if (!orderItems.Any())
            {
                 return BadRequest(new { success = false, message = "No valid items in order." });
            }

            decimal totalAmount = orderItems.Sum(o => o.Price * o.Quantity);

            var order = new Order
            {
                UserId = userId,
                Status = OrderStatus.Pending,
                TotalAmount = totalAmount,
                ShippingAddress = addressString,
                OrderItems = orderItems
            };

            // Atomic operation: both Add Order and Remove Cart Items are committed in one SaveChangesAsync
            // Explicit transaction is not needed and conflicts with Retry Strategy
            await _orderRepository.AddAsync(order);
            
            // Clear the cart after successful order
            await _cartRepository.ClearCartAsync(userId);
            
            await _unitOfWork.SaveChangesAsync();

            return Ok(new { success = true, message = "Order created successfully", data = new { orderId = order.Id } });
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"[CreateOrder] Unauthorized: {ex.Message}");
            return Unauthorized(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[CreateOrder] Error: {ex.Message}");
            Console.WriteLine($"[CreateOrder] StackTrace: {ex.StackTrace}");
            
            // No rollback needed as we didn't start an explicit transaction
            // EF Core will just discard the changes if SaveChangesAsync fails
            
            return StatusCode(500, new { success = false, message = "Internal Server Error during order creation", error = ex.Message });
        }
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
        
        var orderDtos = orders.Select(o => new OrderDto
        {
            Id = o.Id,
            UserId = o.UserId,
            Status = o.Status.ToString(),
            TotalAmount = o.TotalAmount,
            ShippingAddress = o.ShippingAddress,
            CreatedDate = o.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss"),
            Items = o.OrderItems.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                OrderId = oi.OrderId,
                BookId = oi.BookId,
                ProductName = oi.Book?.BookName ?? "Unknown Product",
                BookCoverImage = oi.Book?.CoverImage ?? "",
                Quantity = oi.Quantity,
                Price = oi.Price
            }).ToList()
        }).ToList();
        
        return Ok(new { success = true, message = "Successfully fetched user orders", data = orderDtos });
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

public class OrderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public string CreatedDate { get; set; } = string.Empty;
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int BookId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string BookCoverImage { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
