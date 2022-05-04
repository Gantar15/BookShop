using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.Repositories
{
    public class BasketProductRepository : DbRepository<BasketProduct>
    {
        public override IQueryable<BasketProduct> Items => base.Items.Include(item => item.Basket).Include(item => item.Product);
        public BasketProductRepository(BookShopContext context) : base(context) { }
    }
}
