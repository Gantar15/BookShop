using DataAccess.Entities.Base;

namespace DataAccess
{
    public class BasketProduct : Entity
    {
        public int Id { get; set; }
        public int BasketId { get; set; }
        public Basket Basket { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Count { get; set; }
    }
}
