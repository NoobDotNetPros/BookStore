namespace Bookstore.Models.Entities;

public class Order : BaseAuditableEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public List<OrderItem> OrderItems { get; set; } = new();
}
