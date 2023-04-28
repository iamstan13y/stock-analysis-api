namespace StockAnalysis.API.Models.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICompanyRepository Company { get; }
        IStockRepository Stock { get; }
        void SaveChanges();
    }
}