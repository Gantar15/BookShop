using DataAccess.Repositories;
using System;

namespace DataAccess
{
    public class UnitOfWork : IDisposable
    {
        private readonly BookShopContext db = new BookShopContext();
        private BookRepository bookRepository;
        private BasketRepository basketRepository;
        private BasketProductRepository basketProductRepository;
        private UserRepository userRepository;

        public BookRepository Books
        {
            get
            {
                if (bookRepository == null)
                    bookRepository = new BookRepository(db);
                return bookRepository;
            }
        }
        public BasketRepository Baskets
        {
            get
            {
                if (basketRepository == null)
                    basketRepository = new BasketRepository(db);
                return basketRepository;
            }
        }
        public BasketProductRepository BasketProducts
        {
            get
            {
                if (basketProductRepository == null)
                    basketProductRepository = new BasketProductRepository(db);
                return basketProductRepository;
            }
        }
        public UserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private bool _Disposed;
        protected virtual void Dispose(bool Disposing)
        {
            if (!Disposing || _Disposed) return;
            _Disposed = true;
        }
    }
}
