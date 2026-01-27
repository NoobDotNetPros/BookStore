namespace Bookstore.Models.DTOs;

public class BookDto
{
    public int Id { get; set; }
    public string BookName { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public string CoverImage { get; set; } = string.Empty;
}

public class CreateBookDto
{
    public string BookName { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public string CoverImage { get; set; } = string.Empty;
}

public class UpdateBookDto
{
    public int Id { get; set; }
    public string BookName { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public string CoverImage { get; set; } = string.Empty;
}
