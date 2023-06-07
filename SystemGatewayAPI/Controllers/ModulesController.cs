using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemGateway.Dtos.Entities;
using SystemGateway.Providers;

namespace SystemGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly IServiceAggregator ServiceAggregator;
        public ModulesController(IServiceAggregator aggregator)
        {
            this.ServiceAggregator = aggregator;
        }
        [HttpPost]
        public async Task<IActionResult> RegisterModule([FromBody] Module module)
        {
            if (await ServiceAggregator.DatabaseProvider.FindModuleById(module.Id.ToString()) != null)
                return BadRequest();

            if (!await ServiceAggregator.OperationsManagerProvider.RegisterModule(module)) return BadRequest();
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateModule([FromBody] UpdateModuleInputDto updatedModule)
        {
            if (await ServiceAggregator.DatabaseProvider.FindModuleById(module.Id.ToString()) != null)
                return BadRequest();

            if (!await ServiceAggregator.OperationsManagerProvider.RegisterModule(module)) return BadRequest();
            return Ok();
        }
        //public Task<bool> RegisterModule(Module module);
        //public Task<bool> UpdateModule(string ModuleId, Module module);
        //public Task<bool> DeleteModule(string ModuleId);

    }
}
