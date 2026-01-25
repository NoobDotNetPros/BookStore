using Bookstore.Domain.Common;
using Bookstore.Domain.Enums;

namespace Bookstore.Domain.Entities
{
    public class User:BaseAuditableEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; } = false;
        public UserRole Role { get; set; }
        public List<Address> Addresses { get; set; } = new();
        public List<CartItem> CartItems { get; set; } = new();
        public List<Order> Orders { get; set; } = new();

    }
}
