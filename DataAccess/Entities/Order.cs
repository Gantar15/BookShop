using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Order
    {
        public int Id { get; set; }
        public string Address { get; set; }

        public List<Product> Products { get; set; } = new();
    }
}
