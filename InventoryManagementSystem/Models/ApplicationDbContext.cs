using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<LocationStock> LocationStocks { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        public DbSet<ProductLocationStock> ProductLocationsStocks { get; set; }
    }
}
