using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.Repositories
{
    public class OrderRepository : DbRepository<Order>
    {
        public override IQueryable<Order> Items => base.Items.Include(item => item.Products).Include(item => item.User);
        public OrderRepository(BookShopContext context) : base(context) { }
    }
}
