using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Order
    {
        public Order()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int Id { get; set; }
        public string Address { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
