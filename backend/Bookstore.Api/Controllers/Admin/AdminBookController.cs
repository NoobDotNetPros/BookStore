using Bookstore.Application.Features.Books.Commands.CreateBook;
using Bookstore.Application.Features.Books.Commands.UpdateBook;
using Bookstore.Application.Features.Books.Commands.DeleteBook;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Admin;

[ApiController]
[Route("bookstore_user/admin")]
[Tags("Admin-Product")]
public class AdminBookController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminBookController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Add new product in system
    /// </summary>
    [HttpPost("add/book")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddBook([FromBody] CreateBookCommand command)
    {
        var bookId = await _mediator.Send(command);
        return Ok(new { id = bookId, message = "New product created" });
    }

    /// <summary>
    /// Update product by id
    /// </summary>
    [HttpPut("update/book/{product_id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBook(int product_id, [FromBody] UpdateBookRequest request)
    {
        var command = new UpdateBookCommand(
            product_id,
            request.BookName,
            request.Author,
            request.Description,
            request.Quantity,
            request.Price,
            request.DiscountPrice
        );

        await _mediator.Send(command);
        return Ok(new { message = "Verified successfully" });
    }

    /// <summary>
    /// Delete product by id
    /// </summary>
    [HttpDelete("delete/book/{product_id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBook(int product_id)
    {
        await _mediator.Send(new DeleteBookCommand(product_id));
        return Ok(new { message = "Verified successfully" });
    }
}

public record UpdateBookRequest(
    string BookName,
    string Author,
    string Description,
    int Quantity,
    decimal Price,
    decimal DiscountPrice
);
