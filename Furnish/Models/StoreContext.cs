using Microsoft.EntityFrameworkCore;

namespace Furnish.Models
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Name = "Navi", CategoryId = "Sofa", Price = 649.99 },
                new Product { ProductId = 2, Name = "Serta Peninsula", CategoryId = "Bed", Price = 899.99 },
                new Product { ProductId = 3, Name = "Mobili Fiver", CategoryId = "Table", Price = 945.99 },
                new Product { ProductId = 4, Name = "Ashley Cashton", CategoryId = "Chair", Price = 2299.99 },
                new Product { ProductId = 5, Name = "Llappuil", CategoryId = "Sofa", Price = 649.99 },
                new Product { ProductId = 6, Name = "Yuhuashi", CategoryId = "Bed", Price = 247.00 }
                );
        }
    }
}
