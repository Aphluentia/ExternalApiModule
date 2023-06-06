using PublicAPI.Models.Dtos;
using PublicAPI.Models.Dtos.Base;
using PublicAPI.Operations;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Controllers
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