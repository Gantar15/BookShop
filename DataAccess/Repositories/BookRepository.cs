using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
    public class BookRepository : IRepository<Book>
    {
        private BookShopContext db;
        public BookRepository(BookShopContext context)
        {
            db = context;
        }

        public void Create(Book item)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public Book GetBook(int id)
        {
            db.Books.Include(b => b.Authors).Include(b => b.Photos).Include(b => b.Product);
            var book = db.Books.Find(id);
            return book;
        }

        public IEnumerable<Book> GetBookList()
        {
            throw new System.NotImplementedException();
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

        public void Update(Book item)
        {
            throw new System.NotImplementedException();
        }
    }
}
