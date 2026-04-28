using Microsoft.EntityFrameworkCore;
using FinancialTracker.Models;

namespace FinancialTracker.Data;

/// <summary>
/// Entity Framework database context for the Financial Tracker application.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Stock> Stocks => Set<Stock>();
    public DbSet<StockPrice> StockPrices => Set<StockPrice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Stock configuration
        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Symbol).IsRequired().HasMaxLength(10);
            entity.Property(s => s.CompanyName).IsRequired().HasMaxLength(100);
            entity.HasIndex(s => s.Symbol).IsUnique();
        });

        // StockPrice configuration
        modelBuilder.Entity<StockPrice>(entity =>
        {
            entity.HasKey(sp => sp.Id);
            entity.Property(sp => sp.OpenPrice).HasColumnType("decimal(18,4)");
            entity.Property(sp => sp.ClosePrice).HasColumnType("decimal(18,4)");
            entity.Property(sp => sp.HighPrice).HasColumnType("decimal(18,4)");
            entity.Property(sp => sp.LowPrice).HasColumnType("decimal(18,4)");

            entity.HasOne(sp => sp.Stock)
                  .WithMany(s => s.PriceHistory)
                  .HasForeignKey(sp => sp.StockId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
