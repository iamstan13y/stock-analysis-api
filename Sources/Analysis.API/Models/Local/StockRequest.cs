using StockAnalysis.API.Enums;

namespace StockAnalysis.API.Models.Local
{
    public class StockRequest
    {
        public string? CompanyName { get; set; }
        public string? CompanyCode { get; set; }
        public double UnitPrice { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public StockCategory Category { get; set; }
        public double PercentageRisk { get; set; }
    }
}