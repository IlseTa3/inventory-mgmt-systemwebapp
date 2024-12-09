using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.ViewModels.Registrations;
using InventoryManagementSystem.ViewModels.ProductLocationStock;

namespace InventoryManagementSystem.Models
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<LocationStock> LocationStocks { get; set; }

        public DbSet<ProductLocationStock> ProductLocationsStocks { get; set; }
        public DbSet<InventoryManagementSystem.ViewModels.Registrations.IndexViewModel> IndexViewModel { get; set; } = default!;
        public DbSet<InventoryManagementSystem.ViewModels.ProductLocationStock.IndexViewModel> IndexViewModel_1 { get; set; } = default!;
    }
}
