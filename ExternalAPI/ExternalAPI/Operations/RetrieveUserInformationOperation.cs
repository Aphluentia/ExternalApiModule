using ExternalAPI.Models.DatabaseDtos;
using ExternalAPI.Models.Dtos;
using ExternalAPI.Models.Dtos.Broker;
using ExternalAPI.Models.Dtos.Users;
using ExternalAPI.Models.Entities;
using ExternalAPI.Services.Base;
using Newtonsoft.Json;

namespace ExternalAPI.Operations
{
    public class RetrieveUserInformationOperation : ApplicationBase<RetrieveUserInfoInputDto, RetrieveUserInfoOutputDto>
    {
        private readonly IServiceAggregator _ServiceAggregator;
        public RetrieveUserInformationOperation(ServiceAggregator ServiceAggregator)
        {
            this._ServiceAggregator = ServiceAggregator;
        }
        public async Task<OutputMessage<RetrieveUserInfoOutputDto>> Init(RetrieveUserInfoInputDto input)
        {
            return await base.Init(input);
        }
        public async override Task<OutputMessage<RetrieveUserInfoOutputDto>> Run(RetrieveUserInfoInputDto input)
        {
            var (UserEmail, PermissionLevel, WebPlatformId) = _ServiceAggregator.SessionProvider.GetClaims(input.Token);

            var (success, data) = await _ServiceAggregator.DatabaseProvider.Get($"/User/{UserEmail}");
            if (!success || string.IsNullOrEmpty(data))
                return OutputMessage<RetrieveUserInfoOutputDto>.GetOutputMessage().AddError(ApplicationErrors.FailedToCreateUser);

            return OutputMessage<RetrieveUserInfoOutputDto>.GetOutputMessage(new RetrieveUserInfoOutputDto { User = JsonConvert.DeserializeObject<UserDto>(data) });
        }

               
        

        public override (bool, Error?) ValidateInput(RetrieveUserInfoInputDto input)
        {
            if (string.IsNullOrEmpty(input.Token))
                return (true, ApplicationErrors.AuthenticationTokenIsRequired);
            if (!_ServiceAggregator.SessionProvider.ValidateToken(input.Token))
                return (true, ApplicationErrors.UserAuthenticationFailed);
            return (false, null);
        }
    }
}
