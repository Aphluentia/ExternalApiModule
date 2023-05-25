using ExternalAPI.Models.Dtos.Broker;
using ExternalAPI.Models.Dtos;
using ExternalAPI.Models.Dtos.Modules;
using ExternalAPI.Services.Base;
using ExternalAPI.Models.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using ExternalAPI.Models.DatabaseDtos;
using ExternalAPI.Models.Enums;

namespace ExternalAPI.Operations
{
    public class RetrieveModuleDataOperation: ApplicationBase<RetrieveModuleDataInputDto, RetrieveModuleDataOutputDto>
    {
        private readonly IServiceAggregator _ServiceAggregator;
        public RetrieveModuleDataOperation(ServiceAggregator ServiceAggregator)
        {
            this._ServiceAggregator = ServiceAggregator;
        }
        public async Task<OutputMessage<RetrieveModuleDataOutputDto>> Init(RetrieveModuleDataInputDto input)
        {
            return await base.Init(input);
        }

        public async override Task<OutputMessage<RetrieveModuleDataOutputDto>> Run(RetrieveModuleDataInputDto input)
        {
            
            var (success, moduleString) = await _ServiceAggregator.DatabaseProvider.Get($"/Modules/{input.ModuleId}");
            if (!success || string.IsNullOrEmpty(moduleString))
                return OutputMessage<RetrieveModuleDataOutputDto>.GetOutputMessage().AddError(ApplicationErrors.FailedToCallDatabase);
            var module = JsonConvert.DeserializeObject<ModuleDto>(moduleString);
           
            return OutputMessage<RetrieveModuleDataOutputDto>.GetOutputMessage(new RetrieveModuleDataOutputDto
            {
                Id = module.Id,
                Data = JsonDocument.Parse(module.Data.Substring(1)),
                ModuleType = module.ModuleType,
                Checksum = module.Checksum,
                DateTime = (DateTime)module.DateTime
            });
        }

        public override (bool, Error?) ValidateInput(RetrieveModuleDataInputDto input)
        {
            return (false, null);
        }
    }
}
