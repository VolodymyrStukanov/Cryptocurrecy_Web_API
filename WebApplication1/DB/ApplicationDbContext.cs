using Microsoft.EntityFrameworkCore;
using WebApplication1.DB.Models;

namespace WebApplication1.DB
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Currency>(prop =>
            {
                prop.HasKey(x => x.AssetId);

                prop.HasData(new Currency() { AssetId = "BTC", Name = "Bitcoin" });
                prop.HasData(new Currency() { AssetId = "LTC", Name = "Litecoin" });
                prop.HasData(new Currency() { AssetId = "MSC", Name = "Omni" });
                prop.HasData(new Currency() { AssetId = "STC", Name = "SwiftCoin" });
            });
        }

        public DbSet<Currency> Currencies { get; set; }
    }
}
