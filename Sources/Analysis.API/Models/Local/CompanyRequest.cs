namespace StockAnalysis.API.Models.Local
{
    public class CompanyRequest
    {
        public string? Name { get; set; }
    }

    public class UpdateCompanyRequest : CompanyRequest
    {
        public int Id { get; set; }
    }
}