using ExternalAPI.Models.Dtos;
using ExternalAPI.Models.Dtos.Broker;
using ExternalAPI.Models.Entities;
using ExternalAPI.Services.Base;

namespace ExternalAPI.Operations
{
    public class CloseModuleConnectionOperation : ApplicationBase<CloseModuleConnectionInputDto, CloseModuleConnectionOutputDto>
    {
        private readonly IServiceAggregator _ServiceAggregator;
        public CloseModuleConnectionOperation(ServiceAggregator ServiceAggregator)
        {
            this._ServiceAggregator = ServiceAggregator;
        }
        public async Task<OutputMessage<CloseModuleConnectionOutputDto>> Init(CloseModuleConnectionInputDto input)
        {
            return await base.Init(input);
        }

        public async override Task<OutputMessage<CloseModuleConnectionOutputDto>> Run(CloseModuleConnectionInputDto input)
        {
            var (userEmail, PermissionLevel, WebPlatformId) = _ServiceAggregator.SessionProvider.GetClaims(input.Token);

            var (success, userDto) = await _ServiceAggregator.DatabaseProvider.Delete($"/User/{userEmail}/Connection?ModuleId={input.ModuleId}");
            if (!success)
                return OutputMessage<CloseModuleConnectionOutputDto>.GetOutputMessage().AddError(ApplicationErrors.FailedToCallDatabase);

            var (brokerSuccess, data) = await _ServiceAggregator.BrokerProvider.CloseConnection(input.ModuleId, WebPlatformId, input.ModuleType);
            if (!brokerSuccess || string.IsNullOrEmpty(data))
                return OutputMessage<CloseModuleConnectionOutputDto>.GetOutputMessage().AddError(ApplicationErrors.ConnectionWithBrokerFailed);

            return OutputMessage<CloseModuleConnectionOutputDto>.GetOutputMessage(new CloseModuleConnectionOutputDto { Success = true });
        }

        public override (bool, Error?) ValidateInput(CloseModuleConnectionInputDto input)
        {
            if (string.IsNullOrEmpty(input.Token))
                return (true, ApplicationErrors.AuthenticationTokenIsRequired);
            if (!_ServiceAggregator.SessionProvider.ValidateToken(input.Token))
                return (true, ApplicationErrors.UserAuthenticationFailed);
            return (false, null);
        }
    
    }
}
