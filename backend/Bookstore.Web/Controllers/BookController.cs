using Bookstore.Business.Services.Books.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Controllers;

[ApiController]
[Route("bookstore_user")]
[Tags("Product")]
public class BookController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    [HttpGet("get/book")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _mediator.Send(new GetBooksQuery());
        return Ok(new { message = "Successfully fetched all products", data = books });
    }
}
