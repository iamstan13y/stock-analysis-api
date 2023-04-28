namespace StockAnalysis.API.Models.Repository.IRepository
{
    public interface IUnitOfWork
    {
        void SaveChanges();
    }
}