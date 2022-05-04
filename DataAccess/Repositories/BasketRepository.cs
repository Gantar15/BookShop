using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.Repositories
{
    public class BasketRepository : DbRepository<Basket>
    {
        public override IQueryable<Basket> Items => base.Items.Include(item => item.User).Include(item => item.Products);
        public BasketRepository(BookShopContext context) : base(context) { }
    }
}
