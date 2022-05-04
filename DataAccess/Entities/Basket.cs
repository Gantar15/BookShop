using DataAccess.Entities.Base;
using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Basket : Entity
    {
        public int Id { get; set; }
        public int? UserId { get; set; }

        public User User { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<BasketProduct> BasketProducts { get; set; } = new();
    }
}
