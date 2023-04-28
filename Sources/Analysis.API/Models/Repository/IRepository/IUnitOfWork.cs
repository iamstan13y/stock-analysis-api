namespace StockAnalysis.API.Models.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICompanyRepository Company { get; }
        void SaveChanges();
    }
}