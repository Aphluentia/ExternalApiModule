using ExternalAPI.Configurations;
using ExternalAPI.Models.Dtos;
using ExternalAPI.Models.Dtos.Authentication;
using ExternalAPI.Models.Dtos.Base;
using ExternalAPI.Models.Entities;
using ExternalAPI.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ExternalAPI.Models.DatabaseDtos;
using ExternalAPI.Helpers;
using ExternalAPI.Services.Base;

namespace ExternalAPI.Operations
{
    public class GenerateSessionOperation : ApplicationBase<GenerateSessionInputDto, GenerateSessionOutputDto>
    {
        private readonly IServiceAggregator _ServiceAggregator;
        public GenerateSessionOperation(ServiceAggregator ServiceAggregator) {
            this._ServiceAggregator = ServiceAggregator;
        }
        public async Task<OutputMessage<GenerateSessionOutputDto>> Init(GenerateSessionInputDto generateSessionInputDto)
        {
            return await base.Init(generateSessionInputDto);
        }

     
        public override (bool, Error?) ValidateInput(GenerateSessionInputDto input)
        {
            if (string.IsNullOrEmpty(input.Email))
            {
                return (false, ApplicationErrors.EmailIsRequired);
            }
            if (string.IsNullOrEmpty(input.Password))
            {
                return (false, ApplicationErrors.PasswordIsRequired);
            }
            return (false, null);
        }

        public async override Task<OutputMessage<GenerateSessionOutputDto>> Run(GenerateSessionInputDto input)
        {
            var (success ,data) = await _ServiceAggregator.DatabaseProvider.Get($"/User/{input.Email}");
            if (!success || string.IsNullOrEmpty(data))
                return OutputMessage<GenerateSessionOutputDto>.GetOutputMessage().AddError(ApplicationErrors.UserNotFound);
           
            var userDto = JsonConvert.DeserializeObject<UserDto>(data);

            if (!AuthenticationHelper.AuthenticateUser(userDto.Password, input.Password))
            {
                return OutputMessage<GenerateSessionOutputDto>.GetOutputMessage().AddError(ApplicationErrors.UserAuthenticationFailed);
            }

            var result = _ServiceAggregator.SessionProvider.CreateToken(userDto);
            return OutputMessage<GenerateSessionOutputDto>.GetOutputMessage( new GenerateSessionOutputDto { Token = result});
        }

    }
}
