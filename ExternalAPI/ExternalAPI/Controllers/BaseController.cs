using ExternalAPI.Models.Dtos;
using ExternalAPI.Models.Dtos.Base;
using ExternalAPI.Operations;
using Microsoft.AspNetCore.Mvc;

namespace ExternalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
       
        private readonly ILogger<BaseController> _logger;

        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }
        [HttpPost("Health")]
        public async Task<OutputMessage<HealthOutputDto>> Health([FromBody]HealthInputDto healthInputDto) => await (new HealthOperation()).Init(healthInputDto);
    }
}