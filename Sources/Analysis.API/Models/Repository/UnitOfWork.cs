using StockAnalysis.API.Models.Data;
using StockAnalysis.API.Models.Repository.IRepository;

namespace StockAnalysis.Models.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public void SaveChanges() => _context.SaveChanges();
    }
}