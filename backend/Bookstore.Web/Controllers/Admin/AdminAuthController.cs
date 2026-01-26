using Bookstore.Business.Services.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Controllers.Admin;

[ApiController]
[Route("bookstore_user/admin")]
[Tags("Admin")]
public class AdminAuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminAuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create new admin-user in system
    /// </summary>
    [HttpPost("registration")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return Ok(new { id = userId, message = "New admin-user created" });
    }

    /// <summary>
    /// Admin-user login to system
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginAdmin([FromBody] LoginRequest request)
    {
        // TODO: Implement login logic in Application layer
        return Ok(new { message = "Successfully logged in", email = request.Email });
    }
}

public record LoginRequest(string Email, string Password);
