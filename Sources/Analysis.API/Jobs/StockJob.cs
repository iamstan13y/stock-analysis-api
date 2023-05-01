using Quartz;
using StockAnalysis.API.Models.Data;

namespace StockAnalysis.API.Jobs
{
    [DisallowConcurrentExecution]
    public class StockJob : IJob
    {
        private readonly AppDbContext _context;

        public StockJob(AppDbContext context) => _context = context;

        public Task Execute(IJobExecutionContext context)
        {
            var latestStocks = _context.Stocks!
                .OrderByDescending(x => x.ClosingDate)
                .Take(10)
                .ToList();

            var newStocks = new List<Stock>();
            latestStocks.ForEach(stock =>
            {
                var lastClosingPrice = stock.ClosingPrice;
                var newClosingPrice = PrognosticateClosingPrice(lastClosingPrice);

                newStocks.Add(new Stock
                {
                    CompanyId = stock.CompanyId,
                    ClosingPrice = newClosingPrice,
                    ClosingDate = DateTime.Now,
                    PercentageChange = ((newClosingPrice - lastClosingPrice) / lastClosingPrice) * 100
                });
            });

            _context.Stocks!.AddRange(newStocks);
            _context.SaveChanges();

            return Task.CompletedTask;
        }

        private static double PrognosticateClosingPrice(double closingPrice) =>
            (closingPrice * 0.9) + (new Random().NextDouble() * ((closingPrice * 1.1) - (closingPrice * 0.9)));
    }
}