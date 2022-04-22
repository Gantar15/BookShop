using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Basket
    {
        public Basket()
        {
            BasketProducts = new HashSet<BasketProduct>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<BasketProduct> BasketProducts { get; set; }
    }
}
