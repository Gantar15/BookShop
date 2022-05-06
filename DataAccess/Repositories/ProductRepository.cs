using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.Repositories
{
    public class ProductRepository : DbRepository<Product>
    {
        public override IQueryable<Product> Items => base.Items.Include(item => item.Book).Include(item => item.Baskets).Include(item => item.Orders);
        public ProductRepository(BookShopContext context) : base(context) { }
    }
}