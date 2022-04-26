using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.Repositories
{
    public class BookRepository : DbRepository<Book>
    {
        public override IQueryable<Book> Items => base.Items.Include(item => item.Photos).Include(item => item.Authors).Include(item => item.Category).Include(item => item.Product);
        public BookRepository(BookShopContext context) : base(context) { }
    }
}
