using PublicAPI.Models.Dtos;
using PublicAPI.Models.Dtos.Broker;
using PublicAPI.Models.Dtos.Modules;
using PublicAPI.Models.Entities;
using PublicAPI.Services.Base;
using Newtonsoft.Json;

namespace PublicAPI.Operations
{
    public class RegisterModuleOperation : ApplicationBase<RegisterModuleInputDto, RegisterModuleOutputDto>
    {
        private readonly IServiceAggregator _ServiceAggregator;
        public RegisterModuleOperation(ServiceAggregator ServiceAggregator)
        {
            this._ServiceAggregator = ServiceAggregator;
        }
        
        public async Task<OutputMessage<RegisterModuleOutputDto>> Init(RegisterModuleInputDto input)
        {
            return await base.Init(input);
        }
        public async override Task<OutputMessage<RegisterModuleOutputDto>> Run(RegisterModuleInputDto input)
        {

            var module = new Module
            {
                Data = input.Module.Data,
                ModuleType = input.Module.ModuleType,
                Id = Guid.NewGuid()
            };
            var (success, _) = await _ServiceAggregator.DatabaseProvider.Post($"/Modules", module);
            if (!success)
                return OutputMessage<RegisterModuleOutputDto>.GetOutputMessage().AddError(ApplicationErrors.FailedToCallDatabase);
           return OutputMessage<RegisterModuleOutputDto>.GetOutputMessage(new RegisterModuleOutputDto { Module = module });
        }

        public override (bool, Error?) ValidateInput(RegisterModuleInputDto input)
        {
            return (false, null);
        }
    }
}
