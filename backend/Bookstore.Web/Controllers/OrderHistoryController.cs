using AutoMapper;
using Bookstore.Business.Interfaces;
using Bookstore.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Controllers;

[ApiController]
[Route("api/order-history")]
[Tags("Order History")]
public class OrderHistoryController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderHistoryController(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Get the order history for the current logged-in user
    /// </summary>
    /// <returns>A list of past orders with book details</returns>
    [HttpGet("")]
    [ProducesResponseType(typeof(IEnumerable<OrderHistoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOrderHistory()
    {
        try
        {
            // TODO: Extract userId from JWT Claim (User.Identity.Name or specific claim)
            // For now, using hardcoded userId = 1 as per current project convention
            int userId = 1;

            var orders = await _orderRepository.GetUserOrdersAsync(userId);
            
            var orderHistory = _mapper.Map<IEnumerable<OrderHistoryDto>>(orders);

            return Ok(new 
            { 
                success = true, 
                message = "Order history retrieved successfully", 
                data = orderHistory 
            });
        }
        catch (Exception ex)
        {
            // Log it properly in a real app
            return StatusCode(StatusCodes.Status500InternalServerError, new 
            { 
                success = false, 
                message = "An error occurred while fetching order history",
                error = ex.Message 
            });
        }
    }
}
