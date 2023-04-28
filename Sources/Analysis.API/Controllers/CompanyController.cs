using Microsoft.AspNetCore.Mvc;
using StockAnalysis.API.Models.Data;
using StockAnalysis.API.Models.Local;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _unitOfWork.Company.FindAsync(id);
            if (!result.Success) return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CompanyRequest request)
        {
            var result = await _unitOfWork.Company.AddAsync(new Company
            {
                Name = request.Name
            });
            
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }
    }
}