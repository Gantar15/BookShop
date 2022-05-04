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
        private RoleRepository roleRepository;
        private AuthorRepository authorRepository;
        private OrderProductRepository orderProductRepository;
        private CategoryRepository categoryRepository;
        private OrderRepository orderRepository;
        private PhotoRepository photoRepository;

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
        public RoleRepository Roles
        {
            get
            {
                if (roleRepository == null)
                    roleRepository = new RoleRepository(db);
                return roleRepository;
            }
        }
        public AuthorRepository Authors
        {
            get
            {
                if (authorRepository == null)
                    authorRepository = new AuthorRepository(db);
                return authorRepository;
            }
        }
        public OrderProductRepository OrderProducts
        {
            get
            {
                if (orderProductRepository == null)
                    orderProductRepository = new OrderProductRepository(db);
                return orderProductRepository;
            }
        }
        public CategoryRepository Categories
        {
            get
            {
                if (categoryRepository == null)
                    categoryRepository = new CategoryRepository(db);
                return categoryRepository;
            }
        }
        public OrderRepository Orders
        {
            get
            {
                if (orderRepository == null)
                    orderRepository = new OrderRepository(db);
                return orderRepository;
            }
        }
        public PhotoRepository Photos
        {
            get
            {
                if (photoRepository == null)
                    photoRepository = new PhotoRepository(db);
                return photoRepository;
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
