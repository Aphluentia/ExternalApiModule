using PublicAPI.Controllers;
using PublicAPI.Models.Dtos;
using PublicAPI.Models.Dtos.Authentication;
using PublicAPI.Models.Dtos.Base;
using PublicAPI.Models.Entities;

namespace PublicAPI.Operations
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
