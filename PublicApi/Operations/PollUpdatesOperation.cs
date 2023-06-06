using PublicAPI.Models.DatabaseDtos;
using PublicAPI.Models.Dtos;
using PublicAPI.Models.Dtos.Base;
using PublicAPI.Models.Dtos.Broker;
using PublicAPI.Models.Dtos.Modules;
using PublicAPI.Models.Entities;
using PublicAPI.Services.Base;
using Newtonsoft.Json;

namespace PublicAPI.Operations
{
    public class PollUpdatesOperation : ApplicationBase<PollInputDto, PollOutputDto>
    {
        private readonly IServiceAggregator _ServiceAggregator;
        public PollUpdatesOperation(ServiceAggregator serviceAggregator) { _ServiceAggregator = serviceAggregator; }
        public async Task<OutputMessage<PollOutputDto>> Init(PollInputDto dto)
        {
            return await base.Init(dto);
        }

        public async override Task<OutputMessage<PollOutputDto>> Run(PollInputDto input)
        {
            var (UserEmail, PermissionLevel, WebPlatformId) = _ServiceAggregator.SessionProvider.GetClaims(input.Token);

            var (result, connectionsJson) = await _ServiceAggregator.DatabaseProvider.Get($"/User/{UserEmail}/Connection");
            if (result == false || string.IsNullOrEmpty(connectionsJson))
                return OutputMessage<PollOutputDto>.GetOutputMessage().AddError(ApplicationErrors.FailedToCallDatabase);
            var connections = JsonConvert.DeserializeObject<ISet<string>>(connectionsJson);
            if (connections == null || !connections.Contains(input.ModuleId))
            {
                return OutputMessage<PollOutputDto>.GetOutputMessage().AddError(ApplicationErrors.FailedToPollModuleData);
            }
            // If no poll has been done before
            if (string.IsNullOrEmpty(input.CurrentChecksum))
                return OutputMessage<PollOutputDto>.GetOutputMessage(new PollOutputDto(true));

            // Retrieve module 
            var (success, moduleString) = await _ServiceAggregator.DatabaseProvider.Get($"/Modules/{input.ModuleId}");
            if (!success || string.IsNullOrEmpty(moduleString))
                return OutputMessage<PollOutputDto>.GetOutputMessage().AddError(ApplicationErrors.FailedToCallDatabase);

            var module = JsonConvert.DeserializeObject<ModuleDto>(moduleString);

            if (module.Checksum != input.CurrentChecksum)
            {
                var comparison = module.DateTime > input.CurrentDataTimestamp;
                return OutputMessage<PollOutputDto>.GetOutputMessage(new PollOutputDto(comparison));
            }
                
            return OutputMessage<PollOutputDto>.GetOutputMessage(new PollOutputDto());
        }

        public override (bool, Error?) ValidateInput(PollInputDto input)
        {
            return (false, null);
        }
    }
}
