using System;
using System.Collections.Generic;

namespace Bookstore.Models.DTOs;

public class OrderHistoryDto
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<OrderItemHistoryDto> Items { get; set; } = new();
}

public class OrderItemHistoryDto
{
    public int BookId { get; set; }
    public string BookName { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string CoverImage { get; set; } = string.Empty;
}
