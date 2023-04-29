using Microsoft.EntityFrameworkCore;
using StockAnalysis.API.Models.Data;
using StockAnalysis.API.Models.Local;
using StockAnalysis.API.Models.Repository.IRepository;

namespace StockAnalysis.API.Models.Repository
{
    public class StockRepository : Repository<Stock>, IStockRepository
    {
        public StockRepository(AppDbContext context) : base(context)
        {
        }

        public async new Task<Result<IEnumerable<Stock>>> GetAllAsync() =>
            new Result<IEnumerable<Stock>>(await _dbSet
                .Include(x => x.Company)
                .OrderByDescending(x => x.ClosingDate)
                .ToListAsync());

        public async new Task<Result<Stock>> FindAsync(int id)
        {
            var stock = await _dbSet.FindAsync(id);
            if (stock == null) return new Result<Stock>(false, "Stock not found.");

            return new Result<Stock>(stock);
        }

        public async Task<Result<IEnumerable<Stock>>> GetByCompanyIdAsync(int companyId)
        {
            var stocks = await _dbSet
                .Where(x => x.CompanyId == companyId)
                .Include(x => x.Company)
                .OrderByDescending(x => x.ClosingDate)
                .Take(7)
                .ToListAsync();

            return new Result<IEnumerable<Stock>>(stocks);
        }
    }
}