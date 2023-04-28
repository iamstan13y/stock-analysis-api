using StockAnalysis.API.Models.Data;
using StockAnalysis.API.Models.Repository;
using Microsoft.AspNetCore.Mvc;

namespace StockAnalysis.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackController(IFeedbackRepository feedbackRepository) => _feedbackRepository = feedbackRepository;

        [HttpPost]
        public async Task<IActionResult> Post(int rating) =>
            Ok(await _feedbackRepository.AddAsync(new Feedback { Rating = rating }));

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _feedbackRepository.GetAllAsync());
    }
}