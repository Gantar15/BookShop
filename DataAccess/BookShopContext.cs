using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DataAccess
{
    public partial class BookShopContext : DbContext
    {
        StreamWriter FileLog = new StreamWriter(Directory.GetCurrentDirectory() + @"\Log.txt", true);
        public BookShopContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public BookShopContext(DbContextOptions<BookShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Basket> Baskets { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
                optionsBuilder.UseSqlServer(configuration["connectionStrings:DefaultConnection"]);
                optionsBuilder.LogTo(FileLog.WriteLine, LogLevel.Error);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("Author");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("surname");
            });

            modelBuilder.Entity<Basket>(entity =>
            {
                entity.ToTable("Basket");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(b => b.User)
                    .WithOne(u => u.Basket)
                    .HasForeignKey<Basket>(b => b.UserId)
                    .HasConstraintName("FK_Basket_User");

                entity.HasMany(b => b.Products)
                    .WithMany(p => p.Baskets)
                    .UsingEntity<BasketProduct>(
                        j => j
                            .HasOne(bp => bp.Product)
                            .WithMany(t => t.BasketProducts)
                            .HasForeignKey(bp => bp.ProductId),
                        j => j
                            .HasOne(bp => bp.Basket)
                            .WithMany(p => p.BasketProducts)
                            .HasForeignKey(bp => bp.BasketId),
                        j =>
                        {
                            j.Property(bp => bp.Count).HasDefaultValue(0);
                            j.HasKey(t => t.Id);
                            j.ToTable("BasketProduct");
                        }
                    );
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AgeRestriction)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ageRestriction");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.Description)
                    .HasMaxLength(3200)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Format)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("format");

                entity.Property(e => e.PagesCount).HasColumnName("pagesCount");

                entity.Property(e => e.PublicationYear).HasColumnName("publicationYear");

                entity.Property(e => e.Rating).HasColumnName("rating");
                entity.Property(e => e.AddDate).HasDefaultValueSql("getdate()").HasColumnName("addDate");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.HasOne(b => b.Category)
                    .WithMany(p => p.Books)
                    .HasForeignKey(b => b.CategoryId)
                    .HasConstraintName("FK_Book_Category");

                entity.HasOne(b => b.Product)
                    .WithOne(p => p.Book)
                    .HasForeignKey<Product>(p => p.BookId);

                entity.HasMany(b => b.Authors)
                    .WithMany(p => p.Books)
                    .UsingEntity(j => j.ToTable("BookAuthor"));
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title).HasMaxLength(200);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.HasMany(o => o.Products)
                    .WithMany(p => p.Orders)
                    .UsingEntity<OrderProduct>(
                        j => j
                            .HasOne(op => op.Product)
                            .WithMany(t => t.OrderProducts)
                            .HasForeignKey(op => op.ProductId),
                        j => j
                            .HasOne(op => op.Order)
                            .WithMany(p => p.OrderProducts)
                            .HasForeignKey(op => op.OrderId),
                        j =>
                        {
                            j.Property(op => op.Count).HasDefaultValue(0);
                            j.HasKey(t => t.Id);
                            j.ToTable("OrderProduct");
                        }
                    );
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.ToTable("Photo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookId).HasColumnName("bookId");

                entity.Property(e => e.Source)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("source");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK_Photo_Book");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Role1)
                    .HasMaxLength(100)
                    .HasColumnName("role");

                entity.HasData(
                    new Role { Id = 1, Role1 = "User" },
                    new Role { Id = 2, Role1 = "Admin" }
                );
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Login)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("login");

                entity.Property(e => e.Password)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public override async ValueTask DisposeAsync()
        {
            await FileLog.DisposeAsync();
            await base.DisposeAsync();
        }

        public override void Dispose()
        {
            FileLog.Dispose();
            base.Dispose();
        }
    }
}
