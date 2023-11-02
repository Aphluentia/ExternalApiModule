using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemGatewayAPI.Dtos.Entities.Database;
using SystemGatewayAPI.Dtos.Entities;
using SystemGateway.Providers;

namespace SystemGatewayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IServiceAggregator ServiceAggregator;
        public ApplicationsController(IServiceAggregator aggregator)
        {
            this.ServiceAggregator = aggregator;
        }
        // Operations -----------------------------------------------------------------------------------------------------------
        [HttpPost] // REGISTER_APPLICATION
        public async Task<IActionResult> RegisterApplication([FromBody] Application application)
        {
            var response = await ServiceAggregator.OperationsManagerProvider.RegisterApplication(application);
            if (response.Code != System.Net.HttpStatusCode.OK)
                return BadRequest(response.Message);
            return Ok(response);
        }
        [HttpPost("{ApplicationName}/Version")] // ADD_APPLICATION_VERSION
        public async Task<IActionResult> AddApplicationVersion(string ApplicationName, [FromBody] ModuleVersion version){
            var response = await ServiceAggregator.OperationsManagerProvider.AddApplicationVersion(ApplicationName, version);
            if (response.Code != System.Net.HttpStatusCode.OK)
                return BadRequest(response.Message);
            return Ok(response);
        }
        //[HttpPut("{ApplicationName}/Version/{VersionId}")] // UPDATE_APPLICATION_VERSION
        //public async Task<IActionResult> UpdateApplicationVersion(string ApplicationName, string VersionId, [FromBody] ModuleVersion version)
        //{
        //    var response = await ServiceAggregator.OperationsManagerProvider.UpdateApplicationVersion(ApplicationName, VersionId, version);
        //    if (response.Code != System.Net.HttpStatusCode.OK)
        //        return BadRequest(response.Message);
        //    return Ok(response);
        //}
        //[HttpDelete("{ApplicationName}/Version/{VersionId}")] // DELETE_APPLICATION_VERSION
        //public async Task<IActionResult> DeleteApplicationVersion(string ApplicationName, string VersionId)
        //{
        //    var response = await ServiceAggregator.OperationsManagerProvider.DeleteApplicationVersion(ApplicationName, VersionId);
        //    if (response.Code != System.Net.HttpStatusCode.OK)
        //        return BadRequest(response.Message);
        //    return Ok(response);
        //}
        [HttpDelete("{ApplicationName}")] // DELETE_APPLICATION
        public async Task<IActionResult> DeleteApplication(string ApplicationName)
        {
            var response = await ServiceAggregator.OperationsManagerProvider.DeleteApplication(ApplicationName);
            if (response.Code != System.Net.HttpStatusCode.OK)
                return BadRequest(response.Message);
            return Ok(response);
        }
        // Database Fetch Operations --------------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> FindAllApplications()
        {
            var response = await ServiceAggregator.DatabaseProvider.FindAllApplications();
            if (response == null)
                return BadRequest("Applications Not Found");
            return Ok(response);
        }
        [HttpGet("{ApplicationName}")]
        public async Task<IActionResult> FindApplicationByUd(string ApplicationName)
        {
            var response = await ServiceAggregator.DatabaseProvider.FindApplicationById(ApplicationName);
            if (response == null)
                return BadRequest("Application not Found");
            return Ok(response);
        }
    }
}
