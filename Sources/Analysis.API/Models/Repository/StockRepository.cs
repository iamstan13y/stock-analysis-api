using StockAnalysis.API.Models.Data;
using StockAnalysis.API.Models.Repository.IRepository;

namespace StockAnalysis.API.Models.Repository
{
    public class StockRepository : Repository<Stock>, IStockRepository
    {
        public StockRepository(AppDbContext context) : base(context)
        {
        }
    }
}