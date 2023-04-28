using StockAnalysis.API.Models.Data;
using StockAnalysis.API.Models.Repository.IRepository;

namespace StockAnalysis.API.Models.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(AppDbContext context) : base(context)
        {
        }
    }
}