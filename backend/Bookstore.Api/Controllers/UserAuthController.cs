using Bookstore.Application.Features.Users.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers;

[ApiController]
[Route("bookstore_user")]
[Tags("User")]
public class UserAuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserAuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create new user in system
    /// </summary>
    [HttpPost("registration")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return Ok(new { id = userId, message = "New user created" });
    }

    /// <summary>
    /// Email verification API for the user
    /// </summary>
    [HttpPost("verification/{token}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> VerifyEmail(string token)
    {
        // TODO: Implement verification logic
        return Ok(new { message = "Verified successfully" });
    }

    /// <summary>
    /// User login to system
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginUser([FromBody] LoginRequest request)
    {
        // TODO: Implement login logic
        return Ok(new { message = "Successfully logged in", email = request.Email });
    }
}

public record LoginRequest(string Email, string Password);
