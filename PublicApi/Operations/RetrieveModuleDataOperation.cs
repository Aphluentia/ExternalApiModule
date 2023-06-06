using PublicAPI.Models.Dtos.Broker;
using PublicAPI.Models.Dtos;
using PublicAPI.Models.Dtos.Modules;
using PublicAPI.Services.Base;
using PublicAPI.Models.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using PublicAPI.Models.DatabaseDtos;
using PublicAPI.Models.Enums;

namespace PublicAPI.Operations
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
