using Bookstore.Domain.Common;
using Bookstore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Domain.Entities
{
    public class Order:BaseAuditableEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAmount { get; set; } = string.Empty;
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
