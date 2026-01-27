using Bookstore.Business.Interfaces;
using Bookstore.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Bookstore.DataAccess.Context;

namespace Bookstore.Web.Controllers;

[ApiController]
[Route("api/orders")]
[Tags("Order")]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderController(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Add new order
    /// </summary>
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        // TODO: Get userId from JWT token
        int userId = 1;

        decimal totalAmount = request.Orders.Sum(o => o.Product_Price * o.Product_Quantity);

        var order = new Order
        {
            UserId = userId,
            Status = OrderStatus.Pending,
            TotalAmount = totalAmount,
            ShippingAddress = "Default Address", // TODO: Get from user profile
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
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

        return Ok(new { message = "New order", data = request });
    }

    /// <summary>
    /// Get all user orders
    /// </summary>
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserOrders()
    {
        // TODO: Get userId from JWT token
        int userId = 1;

        var orders = await _orderRepository.GetUserOrdersAsync(userId);
        return Ok(new { message = "Successfully fetched user orders", data = orders });
    }

    /// <summary>
    /// Get order by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            return NotFound(new { message = "Order not found" });

        return Ok(new { message = "Successfully fetched order", data = order });
    }
}

public record CreateOrderRequest(List<OrderItemRequest> Orders);

public record OrderItemRequest(
    string Product_Id,
    string Product_Name,
    int Product_Quantity,
    decimal Product_Price
);
