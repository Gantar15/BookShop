using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.Repositories
{
    public class OrderProductRepository : DbRepository<OrderProduct>
    {
        public override IQueryable<OrderProduct> Items => base.Items.Include(item => item.Order).Include(item => item.Product);
        public OrderProductRepository(BookShopContext context) : base(context) { }
    }
}
