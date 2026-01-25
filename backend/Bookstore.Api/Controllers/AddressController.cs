using Bookstore.Application.Contracts.Repositories;
using Bookstore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers;

[ApiController]
[Route("bookstore_user")]
[Tags("Customer Details")]
public class AddressController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddressController(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Update the customer details to place order
    /// </summary>
    [HttpPut("edit_user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCustomerDetails([FromBody] AddressRequest request)
    {
        // TODO: Get userId from JWT token
        int userId = 1;

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return NotFound(new { message = "User not found" });

        var address = new Address
        {
            UserId = userId,
            AddressType = request.AddressType,
            FullAddress = request.FullAddress,
            City = request.City,
            State = request.State
        };

        user.Addresses.Add(address);
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Ok(new { message = "Customer details added", data = request });
    }
}

public record AddressRequest(
    string AddressType,
    string FullAddress,
    string City,
    string State
);
