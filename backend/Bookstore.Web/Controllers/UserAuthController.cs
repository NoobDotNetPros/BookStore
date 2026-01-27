using Bookstore.Business.Services.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Controllers;

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
        try
        {
            var userId = await _mediator.Send(command);
            return Ok(new
            {
                id = userId,
                message = "Registration successful. Please check your email to verify your account."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
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
        var result = await _mediator.Send(new VerifyEmailCommand(token));

        if (!result)
            return BadRequest(new { message = "Invalid or expired verification token" });

        return Ok(new { message = "Email verified successfully. You can now login." });
    }

    /// <summary>
    /// User login to system
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginUser([FromBody] LoginCommand command)
    {
        try
        {
            var response = await _mediator.Send(command);
            return Ok(new
            {
                message = "Successfully logged in",
                data = response
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
