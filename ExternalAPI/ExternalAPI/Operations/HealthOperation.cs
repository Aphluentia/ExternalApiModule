using ExternalAPI.Controllers;
using ExternalAPI.Models.Dtos;
using ExternalAPI.Models.Dtos.Authentication;
using ExternalAPI.Models.Dtos.Base;
using ExternalAPI.Models.Entities;

namespace ExternalAPI.Operations
{
    public class HealthOperation: ApplicationBase<HealthInputDto, HealthOutputDto>
    {
        public HealthOperation() { }
        public async Task<OutputMessage<HealthOutputDto>> Init(HealthInputDto dto)
        {
            return await base.Init(dto);
        }

        public async override Task<OutputMessage<HealthOutputDto>> Run(HealthInputDto input)
        {
            return OutputMessage<HealthOutputDto>.GetOutputMessage(new HealthOutputDto());
        }

        public override (bool, Error?) ValidateInput(HealthInputDto input)
        {
            return (true, null);
        }
    }
}
