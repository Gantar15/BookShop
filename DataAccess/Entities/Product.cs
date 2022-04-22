using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Product
    {
        public Product()
        {
            BasketProducts = new HashSet<BasketProduct>();
            Books = new HashSet<Book>();
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int Id { get; set; }
        public decimal? Price { get; set; }

        public virtual ICollection<BasketProduct> BasketProducts { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
