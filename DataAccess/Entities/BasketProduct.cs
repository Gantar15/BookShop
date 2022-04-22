using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class BasketProduct
    {
        public int Id { get; set; }
        public int? BasketId { get; set; }
        public int? ProductId { get; set; }

        public virtual Basket Basket { get; set; }
        public virtual Product Product { get; set; }
    }
}
