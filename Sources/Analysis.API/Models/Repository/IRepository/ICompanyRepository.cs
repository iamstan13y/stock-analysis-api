﻿using StockAnalysis.API.Models.Data;
using StockAnalysis.Models.Repository.IRepository;

namespace StockAnalysis.API.Models.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
    }
}