using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.Repositories
{
    public class UserRepository : DbRepository<User>
    {
        public override IQueryable<User> Items => base.Items.Include(item => item.Basket).Include(item => item.Role);
        public UserRepository(BookShopContext context) : base(context) { }
    }
}
