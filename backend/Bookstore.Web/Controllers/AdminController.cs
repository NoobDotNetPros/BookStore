using Bookstore.Business.Interfaces;
using Bookstore.Models.DTOs;
using Bookstore.Models.Entities;
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IBookRepository bookRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("books")]
        public async Task<ActionResult<ApiResponse<List<BookDto>>>> GetAllBooks(CancellationToken cancellationToken = default)
        {
            var books = await _bookRepository.GetAllAsync(cancellationToken);
            var bookDtos = books.Select(b => new BookDto
            {
                Id = b.Id,
                BookName = b.BookName,
                Author = b.Author,
                Description = b.Description,
                ISBN = b.ISBN,
                Quantity = b.Quantity,
                Price = b.Price,
                DiscountPrice = b.DiscountPrice,
                CoverImage = b.CoverImage
            }).ToList();

            return Ok(new ApiResponse<List<BookDto>>
            {
                Success = true,
                Message = "Books retrieved successfully",
                Data = bookDtos
            });
        }

        [HttpGet("books/{id}")]
        public async Task<ActionResult<ApiResponse<BookDto>>> GetBookById(int id, CancellationToken cancellationToken = default)
        {
            var book = await _bookRepository.GetByIdAsync(id, cancellationToken);
            if (book == null)
            {
                return NotFound(new ApiResponse<BookDto>
                {
                    Success = false,
                    Message = "Book not found"
                });
            }

            var bookDto = new BookDto
            {
                Id = book.Id,
                BookName = book.BookName,
                Author = book.Author,
                Description = book.Description,
                ISBN = book.ISBN,
                Quantity = book.Quantity,
                Price = book.Price,
                DiscountPrice = book.DiscountPrice,
                CoverImage = book.CoverImage
            };

            return Ok(new ApiResponse<BookDto>
            {
                Success = true,
                Message = "Book retrieved successfully",
                Data = bookDto
            });
        }

        [HttpPost("books")]
        public async Task<ActionResult<ApiResponse<BookDto>>> CreateBook([FromBody] CreateBookDto createBookDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<BookDto>
                {
                    Success = false,
                    Message = "Invalid book data"
                });
            }

            var book = new Book
            {
                BookName = createBookDto.BookName,
                Author = createBookDto.Author,
                Description = createBookDto.Description,
                ISBN = createBookDto.ISBN,
                Quantity = createBookDto.Quantity,
                Price = createBookDto.Price,
                DiscountPrice = createBookDto.DiscountPrice,
                CoverImage = createBookDto.CoverImage
            };

            await _bookRepository.AddAsync(book, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var bookDto = new BookDto
            {
                Id = book.Id,
                BookName = book.BookName,
                Author = book.Author,
                Description = book.Description,
                ISBN = book.ISBN,
                Quantity = book.Quantity,
                Price = book.Price,
                DiscountPrice = book.DiscountPrice,
                CoverImage = book.CoverImage
            };

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, new ApiResponse<BookDto>
            {
                Success = true,
                Message = "Book created successfully",
                Data = bookDto
            });
        }

        [HttpPut("books/{id}")]
        public async Task<ActionResult<ApiResponse<BookDto>>> UpdateBook(int id, [FromBody] UpdateBookDto updateBookDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid || id != updateBookDto.Id)
            {
                return BadRequest(new ApiResponse<BookDto>
                {
                    Success = false,
                    Message = "Invalid book data"
                });
            }

            var book = await _bookRepository.GetByIdAsync(id, cancellationToken);
            if (book == null)
            {
                return NotFound(new ApiResponse<BookDto>
                {
                    Success = false,
                    Message = "Book not found"
                });
            }

            book.BookName = updateBookDto.BookName;
            book.Author = updateBookDto.Author;
            book.Description = updateBookDto.Description;
            book.ISBN = updateBookDto.ISBN;
            book.Quantity = updateBookDto.Quantity;
            book.Price = updateBookDto.Price;
            book.DiscountPrice = updateBookDto.DiscountPrice;
            book.CoverImage = updateBookDto.CoverImage;

            await _bookRepository.UpdateAsync(book, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var bookDto = new BookDto
            {
                Id = book.Id,
                BookName = book.BookName,
                Author = book.Author,
                Description = book.Description,
                ISBN = book.ISBN,
                Quantity = book.Quantity,
                Price = book.Price,
                DiscountPrice = book.DiscountPrice,
                CoverImage = book.CoverImage
            };

            return Ok(new ApiResponse<BookDto>
            {
                Success = true,
                Message = "Book updated successfully",
                Data = bookDto
            });
        }

        [HttpDelete("books/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteBook(int id, CancellationToken cancellationToken = default)
        {
            var book = await _bookRepository.GetByIdAsync(id, cancellationToken);
            if (book == null)
            {
                return NotFound(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Book not found"
                });
            }

            await _bookRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Message = "Book deleted successfully",
                Data = true
            });
        }
    }
}
