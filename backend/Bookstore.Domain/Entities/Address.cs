using Bookstore.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Domain.Entities
{
    public class Address:BaseAuditableEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string AddressType { get; set; } = string.Empty;
        public string FullAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}
