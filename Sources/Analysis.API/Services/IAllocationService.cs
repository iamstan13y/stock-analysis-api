using StockAnalysis.API.Models.Data;
using StockAnalysis.API.Models.Local;

namespace StockAnalysis.API.Services
{
    public interface IAllocationService
    {
        Task<Result<Allocation>> CalculateAllocationAsync(AllocationRequest request);
    }
}