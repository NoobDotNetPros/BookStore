using Bookstore.Domain.Common;

namespace Bookstore.Domain.Entities
{
    public class CartItem:BaseAuditableEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;
        public int Quantity { get; set; }
        public bool IsWishlist { get; set; }
    }

}
