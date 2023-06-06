using PublicAPI.Models.Dtos;
using PublicAPI.Models.Entities;

namespace PublicAPI
{
    public abstract class ApplicationBase<TInput, TOutput>
    {
        public async Task<OutputMessage<TOutput>> Init(TInput input)
        {
            var (result, error) = this.ValidateInput(input);
            if (result)
            {
                return OutputMessage<TOutput>.GetOutputMessage().AddError(error);
            }
            return await this.Run(input);
        }

        public abstract (bool, Error?) ValidateInput(TInput input);
        public abstract Task<OutputMessage<TOutput>> Run(TInput input);
       
       
    }
}
