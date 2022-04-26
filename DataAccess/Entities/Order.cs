using DataAccess.Entities.Base;
using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Order : Entity
    {
        public int Id { get; set; }
        public string Address { get; set; }

        public List<Product> Products { get; set; } = new();
        public List<OrderProduct> OrderProducts { get; set; } = new();
    }
}
