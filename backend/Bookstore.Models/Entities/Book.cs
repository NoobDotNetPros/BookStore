namespace Bookstore.Models.Entities
{
    public class Book:BaseAuditableEntity
    {
        public string BookName { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public string CoverImage { get; set; } = string.Empty;
        public List<Feedback> Feedbacks { get; set; } = new();

    }
}
