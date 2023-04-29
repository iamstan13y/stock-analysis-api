using Microsoft.AspNetCore.Mvc;
using StockAnalysis.API.Models.Data;
using StockAnalysis.API.Models.Repository.IRepository;

namespace StockAnalysis.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public StockController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _unitOfWork.Stock.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _unitOfWork.Stock.FindAsync(id);
            if (!result.Success) return NotFound(result);

            return Ok(result);
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompanyId(int companyId) => Ok(await _unitOfWork.Stock.GetByCompanyIdAsync(companyId));
    }
}