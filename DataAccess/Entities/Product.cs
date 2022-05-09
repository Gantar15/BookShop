using DataAccess.Entities.Base;
using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Product : Entity
    {        
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int BookId { get; set; }

        public Book Book { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Basket> Baskets { get; set; } = new List<Basket>();
        public List<OrderProduct> OrderProducts { get; set; } = new();
        public List<BasketProduct> BasketProducts { get; set; } = new();
    }
}
