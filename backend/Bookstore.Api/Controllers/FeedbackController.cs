using Bookstore.Application.Contracts.Repositories;
using Bookstore.Domain.Entities;
using Bookstore.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Controllers;

[ApiController]
[Route("bookstore_user")]
[Tags("Feedback")]
public class FeedbackController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FeedbackController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Add comment & rating on product by product_id
    /// </summary>
    [HttpPost("add/feedback/{product_id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddFeedback(int product_id, [FromBody] FeedbackRequest request)
    {
        // TODO: Get userId from JWT token
        int userId = 1;

        var feedback = new Feedback
        {
            UserId = userId,
            BookId = product_id,
            Comment = request.Comment,
            Rating = int.Parse(request.Rating)
        };

        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Successfully added feedback to product" });
    }

    /// <summary>
    /// Get comment & rating of product by product_id
    /// </summary>
    [HttpGet("get/feedback/{product_id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFeedbacks(int product_id)
    {
        var feedbacks = await _context.Feedbacks
            .Include(f => f.User)
            .Where(f => f.BookId == product_id)
            .ToListAsync();

        return Ok(new { message = "Successfully fetched feedbacks of product", data = feedbacks });
    }
}

public record FeedbackRequest(string Comment, string Rating);
