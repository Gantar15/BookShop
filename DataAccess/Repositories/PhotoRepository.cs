using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.Repositories
{
    public class PhotoRepository : DbRepository<Photo>
    {
        public override IQueryable<Photo> Items => base.Items.Include(item => item.Book);
        public PhotoRepository(BookShopContext context) : base(context) { }
    }
}
