using StockAnalysis.API.Enums;

namespace StockAnalysis.API.Models.Local
{
    public class AllocationRequest
    {
        public AccountType AccountType { get; set; }
        public ProfileType ProfileType { get; set; }
        public double StartingAmount { get; set; }
        public int Period { get; set; }
        public List<int>? SelectedCompanies { get; set; }
    }
}