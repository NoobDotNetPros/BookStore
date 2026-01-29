using Bookstore.Business.Interfaces;
using Bookstore.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Controllers;

[ApiController]
[Route("api/users")]
[Tags("Customer Details")]
[Authorize]
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
    [HttpPut("profile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateCustomerDetails([FromBody] AddressRequest request)
    {
        // Get userId from JWT token in HttpContext
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) 
            ?? User.FindFirst("sub");
        
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized(new { message = "Invalid or missing user token" });

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

    /// <summary>
    /// Get user profile
    /// </summary>
    [HttpGet("profile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserProfile()
    {
        // Get userId from JWT token in HttpContext
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) 
            ?? User.FindFirst("sub");
        
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized(new { message = "Invalid or missing user token" });

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return NotFound(new { message = "User not found" });

        // Return user profile without sensitive data
        var profileResponse = new
        {
            id = user.Id,
            fullName = user.FullName,
            email = user.Email,
            phone = user.Phone,
            addresses = user.Addresses.Select(a => new
            {
                id = a.Id,
                addressType = a.AddressType,
                fullAddress = a.FullAddress,
                city = a.City,
                state = a.State
            }).ToList()
        };

        return Ok(new { message = "Successfully fetched user profile", data = profileResponse });
    }

    /// <summary>
    /// Update user profile details (name, email, phone)
    /// </summary>
    [HttpPatch("profile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateProfileRequest request)
    {
        // Get userId from JWT token in HttpContext
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) 
            ?? User.FindFirst("sub");
        
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized(new { message = "Invalid or missing user token" });

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return NotFound(new { message = "User not found" });

        // Update user fields if provided
        if (!string.IsNullOrWhiteSpace(request.FullName))
            user.FullName = request.FullName;
        
        if (!string.IsNullOrWhiteSpace(request.Email))
            user.Email = request.Email;
        
        if (!string.IsNullOrWhiteSpace(request.Phone))
            user.Phone = request.Phone;

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Ok(new { 
            message = "Profile updated successfully", 
            data = new {
                id = user.Id,
                fullName = user.FullName,
                email = user.Email,
                phone = user.Phone
            }
        });
    }

    /// <summary>
    /// Add a new address for the user
    /// </summary>
    [HttpPost("addresses")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddAddress([FromBody] AddressRequest request)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) 
            ?? User.FindFirst("sub");
        
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized(new { message = "Invalid or missing user token" });

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

        return CreatedAtAction(nameof(GetUserProfile), new { 
            message = "Address added successfully", 
            data = new {
                id = address.Id,
                addressType = address.AddressType,
                fullAddress = address.FullAddress,
                city = address.City,
                state = address.State
            }
        });
    }

    /// <summary>
    /// Update an existing address
    /// </summary>
    [HttpPut("addresses/{addressId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateAddress(int addressId, [FromBody] AddressRequest request)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) 
            ?? User.FindFirst("sub");
        
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized(new { message = "Invalid or missing user token" });

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return NotFound(new { message = "User not found" });

        var address = user.Addresses.FirstOrDefault(a => a.Id == addressId);
        if (address == null)
            return NotFound(new { message = "Address not found" });

        address.AddressType = request.AddressType;
        address.FullAddress = request.FullAddress;
        address.City = request.City;
        address.State = request.State;

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Ok(new { 
            message = "Address updated successfully", 
            data = new {
                id = address.Id,
                addressType = address.AddressType,
                fullAddress = address.FullAddress,
                city = address.City,
                state = address.State
            }
        });
    }

    /// <summary>
    /// Delete an address
    /// </summary>
    [HttpDelete("addresses/{addressId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteAddress(int addressId)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) 
            ?? User.FindFirst("sub");
        
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized(new { message = "Invalid or missing user token" });

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return NotFound(new { message = "User not found" });

        var address = user.Addresses.FirstOrDefault(a => a.Id == addressId);
        if (address == null)
            return NotFound(new { message = "Address not found" });

        user.Addresses.Remove(address);
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Ok(new { message = "Address deleted successfully" });
    }
}

public record AddressRequest(
    string AddressType,
    string FullAddress,
    string City,
    string State
);

public record UpdateProfileRequest(
    string? FullName,
    string? Email,
    string? Phone
);
