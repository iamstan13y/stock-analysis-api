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
                .ToListAsync());
}