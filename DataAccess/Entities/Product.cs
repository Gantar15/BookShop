using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Product
    {        
        public int Id { get; set; }
        public decimal? Price { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Basket> Baskets { get; set; } = new List<Basket>();
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
