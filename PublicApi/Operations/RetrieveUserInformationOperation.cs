using PublicAPI.Models.DatabaseDtos;
using PublicAPI.Models.Dtos;
using PublicAPI.Models.Dtos.Broker;
using PublicAPI.Models.Dtos.Users;
using PublicAPI.Models.Entities;
using PublicAPI.Services.Base;
using Newtonsoft.Json;

namespace PublicAPI.Operations
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
