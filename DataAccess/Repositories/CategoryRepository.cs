using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.Repositories
{
    public class CategoryRepository : DbRepository<Category>
    {
        public override IQueryable<Category> Items => base.Items.Include(item => item.Books);
        public CategoryRepository(BookShopContext context) : base(context) { }
    }
}
