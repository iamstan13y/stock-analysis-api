using StockAnalysis.API.Models.Data;
using StockAnalysis.API.Models.Repository;
using StockAnalysis.API.Models.Repository.IRepository;

namespace StockAnalysis.Models.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public ICompanyRepository Company { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            Company = new CompanyRepository(context);

            _context = context;
        }

        public void SaveChanges() => _context.SaveChanges();
    }
}