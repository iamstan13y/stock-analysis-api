using StockAnalysis.API.Models.Data;
using StockAnalysis.API.Models.Local;
using StockAnalysis.Models.Repository.IRepository;

namespace StockAnalysis.API.Models.Repository.IRepository
{
    public interface IStockRepository : IRepository<Stock>
    {
        Task<Result<IEnumerable<Stock>>> GetByCompanyIdAsync(int companyId);
    }
}