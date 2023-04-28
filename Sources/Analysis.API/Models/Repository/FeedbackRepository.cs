using StockAnalysis.API.Models.Data;
using StockAnalysis.API.Models.Local;
using Microsoft.EntityFrameworkCore;

namespace StockAnalysis.API.Models.Repository
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly AppDbContext _context;

        public FeedbackRepository(AppDbContext context) => _context = context;

        public async Task<Result<Feedback>> AddAsync(Feedback feedback)
        {
            await _context.Feedback!.AddAsync(feedback);
            await _context.SaveChangesAsync();

            return new Result<Feedback>(feedback);
        }

        public async Task<Result<FeedbackResponse>> GetAllAsync()
        {
            var feedback = await _context.Feedback!.ToListAsync();
            double totalRating = 0;

            feedback.ForEach(x =>
            {
                totalRating += x.Rating;
            });

            decimal avgRating = (decimal)totalRating / feedback.Count;

            var response = new FeedbackResponse(feedback.Count, Math.Round(avgRating, 1));
            return new Result<FeedbackResponse>(response);
        }
    }
}