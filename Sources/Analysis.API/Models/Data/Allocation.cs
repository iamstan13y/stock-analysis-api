using StockAnalysis.API.Enums;
using StockAnalysis.API.Models.Data;

namespace StockAnalysis.API.Models.Data
{
    public class Allocation
    {
        public AccountType AccountType { get; set; }
        public ProfileType ProfileType { get; set; }
        public double StartingAmount { get; set; }
        public int Period { get; set; }
        public List<Return>? Returns { get; set; }
        public List<AllocationBreakdown> Breakdown { get; set; }
    }
}