using DataAccess.Entities.Base;

namespace DataAccess
{
    public class OrderProduct : Entity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Count { get; set; }
    }
}
