using Microsoft.AspNetCore.Mvc;
using System;
using SystemGateway.Providers;
using SystemGatewayAPI.Dtos;
using SystemGatewayAPI.Dtos.Entities;
using SystemGatewayAPI.Dtos.Entities.Database;

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

        // Operations -----------------------------------------------------------------------------------------------------------
        [HttpPost] // public Task<ActionResponse> RegisterModule(Module Module); CREATE_MODULE
        public async Task<IActionResult> RegisterModule([FromBody] Module module) 
        {
            module.Id = Guid.NewGuid();
            var operation = await this.ServiceAggregator.OperationsManagerProvider.RegisterModule(module);
            if (operation.Code != System.Net.HttpStatusCode.OK)
                return BadRequest(operation.Message);
            return Ok(operation.Message);
        }
        [HttpPut("{ModuleId}")] // public Task<ActionResponse> UpdateModule(string ModuleId, Module module); //UPDATE_MODULE,
        public async Task<IActionResult> UpdateModule(Guid ModuleId, [FromBody] ModuleVersion updatedModule) 
        {
            var module = new Module
            {
                Id = ModuleId,
                ModuleData = updatedModule
            };
            var actionResponse = await this.ServiceAggregator.OperationsManagerProvider.UpdateModule(ModuleId.ToString(), module);
            if (actionResponse.Code != System.Net.HttpStatusCode.OK)
                return BadRequest(actionResponse.Message);
            return Ok("Module updated with Success");
        }

        [HttpPost("{ModuleId}/Profile/{ProfileName}")] // public Task<ActionResponse> UpdateModule(string ModuleId, Module module); //UPDATE_MODULE,
        public async Task<IActionResult> AddNewProfile(Guid ModuleId, string ProfileName)
        {
         
            var actionResponse = await this.ServiceAggregator.OperationsManagerProvider.ModuleAddNewContext(ModuleId.ToString(), ProfileName);
            if (actionResponse.Code != System.Net.HttpStatusCode.OK)
                return BadRequest(actionResponse.Message);
            return Ok("Module updated with Success");
        }

        [HttpDelete("{ModuleId}/Profile/{ProfileName}")] // public Task<ActionResponse> UpdateModule(string ModuleId, Module module); //UPDATE_MODULE,
        public async Task<IActionResult> DeleteProfile(Guid ModuleId, string ProfileName)
        {
            var actionResponse = await this.ServiceAggregator.OperationsManagerProvider.ModuleDeleteContext(ModuleId.ToString(), ProfileName);
            if (actionResponse.Code != System.Net.HttpStatusCode.OK)
                return BadRequest(actionResponse.Message);
            return Ok("Module updated with Success");
        }

        [HttpPut("{ModuleId}/Version/{VersionId}")] //public Task<ActionResponse> UpdateModuleToVersion(string ModuleId, string VersionId); UPDATE_MODULE_TO_VERSION
        public async Task<IActionResult> UpdateModuleToVersion(string ModuleId, string VersionId)
        {
            var actionResponse = await this.ServiceAggregator.OperationsManagerProvider.UpdateModuleToVersion(ModuleId, VersionId);
            if (actionResponse.Code != System.Net.HttpStatusCode.OK)
                return BadRequest(actionResponse.Message);
            return Ok($"Module updated to version {VersionId} with Success");
        }
        [HttpDelete("{ModuleId}")] // public Task<ActionResponse> DeleteModule(string ModuleId); DELETE_MODULE,
        public async Task<IActionResult> DeleteModule(string ModuleId) 
        {
            var actionResponse = await this.ServiceAggregator.OperationsManagerProvider.DeleteModule(ModuleId);
            if (actionResponse.Code != System.Net.HttpStatusCode.OK)
                return BadRequest(actionResponse.Message);
            return Ok($"Module deleted with Success");
        }
        // Database Fetch Operations --------------------------------------------------------------------------------------------
        [HttpGet] 
        public async Task<IActionResult> FindAllModules()
        {
            var response = await this.ServiceAggregator.DatabaseProvider.FindAllModules();
            if (response == null)
                return BadRequest("No Modules Found");
            return Ok(response);
        }

        [HttpGet("{ModuleId}")] 
        public async Task<IActionResult> FindModuleById(Guid ModuleId)
        {
            var response = await this.ServiceAggregator.DatabaseProvider.FindModuleById(ModuleId);
            if (response == null)
                return BadRequest("No Modules Found");
            return Ok(response);
        }

    }
}
