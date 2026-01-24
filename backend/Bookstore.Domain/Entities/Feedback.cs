using Bookstore.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Domain.Entities
{
    public class Feedback:BaseAuditableEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}
