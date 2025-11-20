using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OnlineShop.Db.Models;


namespace OnlineShop.Db
{
    public class DatabaseContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Comparison> Comparisons { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
           // Database.Migrate();
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>().OwnsOne(o => o.Customer);

            modelBuilder.Entity<Order>()
                .Property(o => o.UserId)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Cost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Favorite>()
                .HasMany(f => f.Products)
                .WithMany()
                .UsingEntity("FavoriteProducts");

            modelBuilder.Entity<Comparison>()
                .HasMany(c => c.Products)
                .WithMany()
                .UsingEntity("ComparisonProducts");

            var defaultProducts = new List<Product>
            {
             new Product { Id = 1, Name = "Протеин-супер Optimum Nutrition 990 g", Cost = 8500, Description = "Сывороточный протеин самого высокого класса" },
             new Product { Id = 2, Name = "IGF-1 LR3 1 mg", Cost = 5500, Description = "Инсулиноподобный фактор роста пролонгированного действия индуцирует рост мышц и регенерацию" },
             new Product { Id = 3, Name = "BCAA 500 g", Cost = 3500, Description = "BCAA обеспечивают энергию и защищают мышцы во время тренировки" },
             new Product { Id = 4, Name = "Комплексные аминокислоты 500 g", Cost = 4500, Description = "Аминокислоты ускоряют восстановление мышечной ткани после тренировки" },
             new Product { Id = 5, Name = "Gainer Mutant 5,5 kg", Cost = 5000, Description = "Заменяет собой пищу, обеспечивая энергией и белком после тренировки и при дефиците пищи" },
             new Product { Id = 6, Name = "Creatine citrate 500 g", Cost = 4900, Description = "Креатин цитрат обеспечивает силовую выносливость, лучшую работу мозга и сердца и не вызывает отёков" },
             new Product { Id = 7, Name = "Psychotic предтрен", Cost = 2999, Description = "Предтрен стимулирует нервную систему и улучшает силовую выносливость, что напрямую влияет на физическую производительность в период тренировки." },
             new Product { Id = 8, Name = "Epithalon 10 mg / 2 vials", Cost = 3990, Description = "Препарат активирует фермент теломеразу, индуцирует удлинение теломер, предовращает появление онкологических заболеваний, продляет жизнь и улучшает качество жизни......" }
            };

            modelBuilder.Entity<Product>().HasData(defaultProducts);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Login).HasMaxLength(256); 
                entity.HasIndex(e => e.Login).IsUnique(); 
            });
        }
    }
}
