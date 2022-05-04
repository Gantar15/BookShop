using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.Repositories
{
    public class AuthorRepository : DbRepository<Author>
    {
        public override IQueryable<Author> Items => base.Items.Include(item => item.Books);
        public AuthorRepository(BookShopContext context) : base(context) { }
    }
}
