﻿using ExternalAPI.Models.Dtos.Users;
using ExternalAPI.Models.Dtos;
using ExternalAPI.Operations;
using ExternalAPI.Services.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExternalAPI.Models.Dtos.Modules;

namespace ExternalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly IServiceAggregator _ServiceAggregator;
        public ModulesController(IServiceAggregator ServiceAggregator)
        {
            this._ServiceAggregator = ServiceAggregator;
        }

        // Methods:
        // 1 - Poll Without Data:
        //      Module Calls ExternalAPI, calls DatabaseAPI and retrieves data. Calculates Checksum.
        //      If checksum is the same do nothing. If different, compare timestamp: latest data is saved in the database
        
        [HttpPost("Poll")]
        public async Task<OutputMessage<PollOutputDto>> Poll([FromBody] PollInputDto input) => await (new PollUpdatesOperation((ServiceAggregator)_ServiceAggregator)).Init(input);

        [HttpPost("RegisterModule")]
        public async Task<OutputMessage<RegisterModuleOutputDto>> RegisterModule([FromBody] RegisterModuleInputDto input) => await (new RegisterModuleOperation((ServiceAggregator)_ServiceAggregator)).Init(input);
        [HttpPost("RetrieveModuleData")]
        public async Task<OutputMessage<RetrieveModuleDataOutputDto>> RetrieveModuleData([FromBody] RetrieveModuleDataInputDto input) => await (new RetrieveModuleDataOperation((ServiceAggregator)_ServiceAggregator)).Init(input);
        
        [HttpPost("UpdateModuleData")]
        public async Task<OutputMessage<UpdateModuleDataOutputDto>> UpdateModuleData([FromBody] UpdateModuleDataInputDto input) => await (new UpdateModuleDataOperation((ServiceAggregator)_ServiceAggregator)).Init(input);
        //

    }
}
