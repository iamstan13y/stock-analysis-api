using Microsoft.EntityFrameworkCore;

namespace StockAnalysis.API.Models.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Company>? Companies { get; set; }
        public DbSet<Stock>? Stocks { get; set; }
    }
}