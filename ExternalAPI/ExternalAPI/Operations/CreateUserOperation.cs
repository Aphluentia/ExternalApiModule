using ExternalAPI.Helpers;
using ExternalAPI.Models.Dtos;
using ExternalAPI.Models.Dtos.Users;
using ExternalAPI.Models.Entities;
using ExternalAPI.Services.Base;
using Newtonsoft.Json;

namespace ExternalAPI.Operations
{
    public class CreateUserOperation : ApplicationBase<CreateUserInputDto, CreateUserOutputDto>
    {
        private readonly IServiceAggregator _ServiceAggregator;
        public CreateUserOperation(ServiceAggregator ServiceAggregator)
        {
            this._ServiceAggregator = ServiceAggregator;
        }
        public async Task<OutputMessage<CreateUserOutputDto>> Init(CreateUserInputDto input)
        {
            return await base.Init(input);
        }
        public async override Task<OutputMessage<CreateUserOutputDto>> Run(CreateUserInputDto input)
        {
            input.User.Password = AuthenticationHelper.HashPassword(input.User.Password);
            
            var (success, data) = await _ServiceAggregator.DatabaseProvider.Post($"/User", input.User);
            if (!success || string.IsNullOrEmpty(data))
                return OutputMessage<CreateUserOutputDto>.GetOutputMessage().AddError(ApplicationErrors.FailedToCreateUser);

            return OutputMessage<CreateUserOutputDto>.GetOutputMessage(new CreateUserOutputDto { Success = true });

        }

        public override (bool, Error?) ValidateInput(CreateUserInputDto input)
        {
            return (false, null);
        }
    }
}
