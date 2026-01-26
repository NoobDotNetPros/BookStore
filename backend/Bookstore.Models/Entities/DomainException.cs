namespace Bookstore.Models.Entities
{
    public class DomainException:Exception
    {
        public DomainException(string message):base(message){}
    }
}
