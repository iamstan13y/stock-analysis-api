using Microsoft.EntityFrameworkCore;
using Quartz;
using StockAnalysis.API.Models.Data;

namespace StockAnalysis.API.Jobs
{
    [DisallowConcurrentExecution]
    public class StockJob : IJob
    {
        private readonly AppDbContext _context;

        public StockJob(AppDbContext context) => _context = context;

        public async Task Execute(IJobExecutionContext context)
        {
            var latestStocks = await _context.Stocks!
                .OrderByDescending(x => x.ClosingDate)
                .Take(10)
                .ToListAsync();

            var newStocks = latestStocks.Select(stock =>
            {
                var newClosingPrice = PrognosticateClosingPrice(stock.ClosingPrice);
                var verdicts = new List<string> { "Buy", "Hold", "Sell"};

                return new Stock
                {
                    CompanyId = stock.CompanyId,
                    ClosingPrice = newClosingPrice,
                    ClosingDate = DateTime.Now,
                    PercentageChange = ((newClosingPrice - stock.ClosingPrice) / stock.ClosingPrice) * 100,
                    Verdict = verdicts[new Random().Next(0, 3)]
                };
            }).ToList();

            await _context.Stocks!.AddRangeAsync(newStocks);
            await _context.SaveChangesAsync();
        }

        private static double PrognosticateClosingPrice(double closingPrice) =>
            (closingPrice * 0.9) + (new Random().NextDouble() * ((closingPrice * 1.1) - (closingPrice * 0.9)));
    }
}