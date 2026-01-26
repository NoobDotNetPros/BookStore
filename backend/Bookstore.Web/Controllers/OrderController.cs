using Bookstore.Business.Interfaces;
using Bookstore.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Controllers;

[ApiController]
[Route("bookstore_user")]
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
    [HttpPost("add/order")]
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

        await _orderRepository.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return Ok(new { message = "New order", data = request });
    }
}

public record CreateOrderRequest(List<OrderItemRequest> Orders);

public record OrderItemRequest(
    string Product_Id,
    string Product_Name,
    int Product_Quantity,
    decimal Product_Price
);
