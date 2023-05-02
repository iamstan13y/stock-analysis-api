namespace StockAnalysis.API.Models.Data
{
    public class Stock
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public double ClosingPrice { get; set; }
        public DateTime ClosingDate { get; set; } = DateTime.Now;
        public double PercentageChange { get; set; }
        public string? Verdict { get; set; }
        public Company? Company { get; set; }
    }
}