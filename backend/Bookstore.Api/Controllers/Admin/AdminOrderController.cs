using Bookstore.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Admin;

[ApiController]
[Route("bookstore_user/admin")]
[Tags("Admin-Order")]
public class AdminOrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;

    public AdminOrderController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// Get all orders list
    /// </summary>
    [HttpGet("get/order")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderRepository.GetAllAsync();
        return Ok(new { message = "Successfully fetched all orders list", data = orders });
    }
}
