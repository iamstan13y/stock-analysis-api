using Microsoft.AspNetCore.Mvc;
using StockAnalysis.API.Models.Repository.IRepository;

namespace StockAnalysis.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _unitOfWork.Company.GetAllAsync());
    }
}