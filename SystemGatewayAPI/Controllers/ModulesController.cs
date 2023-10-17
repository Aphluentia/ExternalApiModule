using DatabaseApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using SystemGateway.Providers;
using SystemGatewayAPI.Dtos;

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
      
        [HttpGet("{ModuleId}/Alive/{Checksum}")]
        public async Task<IActionResult> ModuleIsAlive(string ModuleId,string Checksum, string? Token)
        {
            var module = new Module();
            switch (string.IsNullOrEmpty(Token))
            {
                case true:
                    module = await ServiceAggregator.DatabaseProvider.FindModuleById(ModuleId);
                    if (module == null) return NotFound("Application Was not Found");
                    break;
                case false:
                    if (!await this.ServiceAggregator.SecurityManagerProvider.ValidateSession(Token)) return Unauthorized("Unauthorized Request");
                    var secData = (await this.ServiceAggregator.SecurityManagerProvider.GetTokenData(Token));
                    if (secData.IsExpired) return Unauthorized("Token Expired");
                    var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(secData.Email);
                    module = patient?.WebPlatform.Modules.Where(c => c.Id == Guid.Parse(ModuleId)).FirstOrDefault();
                    if (module == null) return NotFound("Application not associated with user");
                    break;
              
            }
            if (module.ModuleTemplate.Checksum != Checksum) return Ok(new ModuleAliveDto { IsAlive = true, ChecksumIsDifferent = true });
            return Ok(new ModuleAliveDto { IsAlive = true, ChecksumIsDifferent = false });
        }
        [HttpGet("{ModuleId}/Updates/{LastUpdateTimestamp}")]
        public async Task<IActionResult> FindModuleUpdates(string ModuleId, DateTime LastUpdateTimestamp, string? Token)
        {
            var module = new Module();
            switch (string.IsNullOrEmpty(Token))
            {
                case true:
                    module = await ServiceAggregator.DatabaseProvider.FindModuleById(ModuleId);
                    if (module == null) return NotFound("Application Was not Found");
                    break;
                case false:
                    if (!await this.ServiceAggregator.SecurityManagerProvider.ValidateSession(Token)) return Unauthorized("Unauthorized Request");
                    var secData = (await this.ServiceAggregator.SecurityManagerProvider.GetTokenData(Token));
                    if (secData.IsExpired) return Unauthorized("Token Expired");
                    var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(secData.Email);
                    module = patient?.WebPlatform.Modules.Where(c => c.Id == Guid.Parse(ModuleId)).FirstOrDefault();
                    if (module == null) return NotFound("Application not associated with user");
                    break;

            }
            if (module.ModuleTemplate.Timestamp > LastUpdateTimestamp) return Ok(new ModuleUpdateDto {HasUpdates = true, ModuleData = module });
            return Ok(new ModuleUpdateDto { HasUpdates = false });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterModule([FromBody] Module module, string? Token)
        {
            var application = this.ServiceAggregator.DatabaseProvider.FindApplicationById(module.ModuleTemplate.ModuleName);
            module.Id = Guid.NewGuid();
            switch (string.IsNullOrEmpty(Token))
            {
                case true:
                    if (!await this.ServiceAggregator.OperationsManagerProvider.RegisterModule(module))
                        return BadRequest("Failed to Create Module");
                    break;
                case false:
                    if (!await this.ServiceAggregator.SecurityManagerProvider.ValidateSession(Token)) return Unauthorized("Unauthorized Request");
                    var secData = (await this.ServiceAggregator.SecurityManagerProvider.GetTokenData(Token));
                    var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(secData.Email);
                    if (patient == null) return NotFound("User Not Found");
                    if (!await this.ServiceAggregator.OperationsManagerProvider.AddNewModuleToPatient(patient.Email, module))
                        return BadRequest($"Failed to Create Module to patient {patient.Email}");

                    break;
            }
            return Ok("Generating Module");
        }
        [HttpGet("Add/{ModuleId}/{Token}")]
        public async Task<IActionResult> RegisterExistingModuleToPatient(string ModuleId, string Token)
        {
            if (string.IsNullOrEmpty(Token)) return BadRequest("Token cannot be empty");
            if (!await this.ServiceAggregator.SecurityManagerProvider.ValidateSession(Token)) return Unauthorized("Unauthorized Request");
            var secData = (await this.ServiceAggregator.SecurityManagerProvider.GetTokenData(Token));

            var module = await ServiceAggregator.DatabaseProvider.FindModuleById(ModuleId);
            if (module == null) return NotFound("Application Was not Found");
            await ServiceAggregator.OperationsManagerProvider.DeleteModule(ModuleId);

            var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(secData.Email);
            if (patient == null) return NotFound("User Not Found");
            if (!await this.ServiceAggregator.OperationsManagerProvider.AddNewModuleToPatient(patient.Email, module))
                return BadRequest($"Failed to Create Module to patient {patient.Email}");
            return Ok();
           
        }
        [HttpPut("{ModuleId}")]
        public async Task<IActionResult> UpdateModule(string ModuleId, string? Token, [FromBody] Module updatedModule)
        {
            switch (string.IsNullOrEmpty(Token))
            {
                case true:
                    if (await this.ServiceAggregator.DatabaseProvider.FindModuleById(ModuleId) == null)
                        return NotFound("Module not Found");
                    if (!await this.ServiceAggregator.OperationsManagerProvider.UpdateModule(ModuleId, updatedModule))
                        return BadRequest("Failed to update Module");
                    break;
                case false:
                    if (!await this.ServiceAggregator.SecurityManagerProvider.ValidateSession(Token)) 
                        return Unauthorized("Unauthorized Request");

                    var secData = (await this.ServiceAggregator.SecurityManagerProvider.GetTokenData(Token));
                    var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(secData.Email);
                    if (patient == null) return NotFound("User Not Found");
                    if (!patient.WebPlatform.Modules.Any(c => c.Id == Guid.Parse(ModuleId))) return NotFound("Module not found");
                    if (!await this.ServiceAggregator.OperationsManagerProvider.UpdatePatientModule(patient.Email, ModuleId, updatedModule))
                        return BadRequest($"Failed to update module data");

                    break;
            }
            return Ok("Updating Module");
        }
        [HttpGet("{ModuleId}/{VersionId}")]
        public async Task<IActionResult> UpdateModuleToVersion(string ModuleId, string VersionId, string? Token)
        {
            switch (string.IsNullOrEmpty(Token))
            {
                case true:
                    if (await this.ServiceAggregator.DatabaseProvider.FindModuleById(ModuleId) == null)
                        return NotFound("Module not Found");
                    if (!await this.ServiceAggregator.OperationsManagerProvider.UpdateModuleToVersion(ModuleId, VersionId))
                        return BadRequest("Failed to update Module");
                    break;
                case false:
                    if (!await this.ServiceAggregator.SecurityManagerProvider.ValidateSession(Token))
                        return Unauthorized("Unauthorized Request");

                    var secData = (await this.ServiceAggregator.SecurityManagerProvider.GetTokenData(Token));
                    var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(secData.Email);
                    if (patient == null) return NotFound("User Not Found");
                    if (!patient.WebPlatform.Modules.Any(c => c.Id == Guid.Parse(ModuleId))) return NotFound("Module not found");
                    if (!await this.ServiceAggregator.OperationsManagerProvider.UpdatePatientModuleToVersion(patient.Email, ModuleId, VersionId))
                        return BadRequest($"Failed to update module data");

                    break;
            }
            return Ok("Updating Module To Version");
        }
        [HttpDelete("{ModuleId}")]
        public async Task<IActionResult> DeleteModule(string ModuleId, string? Token)
        {
            switch (string.IsNullOrEmpty(Token))
            {
                case true:
                    if (await this.ServiceAggregator.DatabaseProvider.FindModuleById(ModuleId) == null)
                        return NotFound("Module not Found");
                    if (!await this.ServiceAggregator.OperationsManagerProvider.DeleteModule(ModuleId))
                        return BadRequest("Failed to delete Module");
                    break;
                case false:
                    if (!await this.ServiceAggregator.SecurityManagerProvider.ValidateSession(Token))
                        return Unauthorized("Unauthorized Request");

                    var secData = (await this.ServiceAggregator.SecurityManagerProvider.GetTokenData(Token));
                    var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(secData.Email);
                    if (patient == null) return NotFound("User Not Found");
                    if (!patient.WebPlatform.Modules.Any(c => c.Id == Guid.Parse(ModuleId))) return NotFound("Module not found");
                    if (!await this.ServiceAggregator.OperationsManagerProvider.DeletePatientModule(patient.Email, ModuleId))
                        return BadRequest($"Failed to delete patient's module");

                    break;
            }
            return Ok("Deleting Module");
        }

    }
}
