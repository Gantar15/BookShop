using System.Linq;

namespace DataAccess.Repositories
{
    public class RoleRepository : DbRepository<Role>
    {
        public override IQueryable<Role> Items => base.Items;
        public RoleRepository(BookShopContext context) : base(context) { }
    }
}
