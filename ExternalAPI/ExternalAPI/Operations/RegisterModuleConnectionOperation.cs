using ExternalAPI.Models.Dtos.Authentication;
using ExternalAPI.Models.Dtos;
using ExternalAPI.Models.Dtos.Base;
using ExternalAPI.Models.Dtos.Broker;
using ExternalAPI.Services.Base;
using ExternalAPI.Models.Entities;
using ExternalAPI.Helpers;
using ExternalAPI.Models.DatabaseDtos;
using Newtonsoft.Json;

namespace ExternalAPI.Operations
{
    public class RegisterModuleConnectionOperation : ApplicationBase<RegisterModuleConnectionInputDto, RegisterModuleConnectionOutputDto>
    {
        private readonly IServiceAggregator _ServiceAggregator;
        public RegisterModuleConnectionOperation(ServiceAggregator ServiceAggregator)
        {
            this._ServiceAggregator = ServiceAggregator;
        }
        public async Task<OutputMessage<RegisterModuleConnectionOutputDto>> Init(RegisterModuleConnectionInputDto input)
        {
           
            return await base.Init(input);
        }

        public async override Task<OutputMessage<RegisterModuleConnectionOutputDto>> Run(RegisterModuleConnectionInputDto input)
        {
            var (userEmail, PermissionLevel, WebPlatformId) = _ServiceAggregator.SessionProvider.GetClaims(input.Token);
            var (success, userDto) = await _ServiceAggregator.DatabaseProvider.Post($"/User/{userEmail}/Connection", input.ModuleId);
            if (!success)
                return OutputMessage<RegisterModuleConnectionOutputDto>.GetOutputMessage().AddError(ApplicationErrors.FailedToCallDatabase);

        

            var (brokerSuccess, data) = await _ServiceAggregator.BrokerProvider.CreateConnection(input.ModuleId, WebPlatformId, input.ModuleType);
            if (!brokerSuccess || string.IsNullOrEmpty(data))
                return OutputMessage<RegisterModuleConnectionOutputDto>.GetOutputMessage().AddError(ApplicationErrors.FailedToCreateConnection);

            return OutputMessage<RegisterModuleConnectionOutputDto>.GetOutputMessage(new RegisterModuleConnectionOutputDto { Success = true});
        }

        public override (bool, Error?) ValidateInput(RegisterModuleConnectionInputDto input)
        {
            if (string.IsNullOrEmpty(input.Token))
                return (true, ApplicationErrors.AuthenticationTokenIsRequired);
            if (!_ServiceAggregator.SessionProvider.ValidateToken(input.Token))
                return (true, ApplicationErrors.UserAuthenticationFailed);

            return (false, null);
        }
    }
}
