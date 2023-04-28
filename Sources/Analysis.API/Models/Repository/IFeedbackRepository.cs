using StockAnalysis.API.Models.Data;
using StockAnalysis.API.Models.Local;

namespace StockAnalysis.API.Models.Repository
{
    public interface IFeedbackRepository
    {
        Task<Result<Feedback>> AddAsync(Feedback feedback);
        Task<Result<FeedbackResponse>> GetAllAsync();
    }
}