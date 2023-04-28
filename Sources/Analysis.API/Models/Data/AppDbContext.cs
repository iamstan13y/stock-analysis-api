using Microsoft.EntityFrameworkCore;

namespace StockAnalysis.API.Models.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Stock>? Stocks { get; set; }
        public DbSet<Feedback>? Feedback { get; set; }
    }
}