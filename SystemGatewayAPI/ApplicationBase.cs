namespace SystemGateway
{

    public abstract class ApplicationBase<TInput, TOutput>
    {
        public async Task<OutputMessage<TOutput>> Init(TInput input)
        {
            var metadata = this.ValidateInput(input);
            if (!metadata.Success)
            {
                return OutputMessage<TOutput>.GetOutputMessage().AddManyErrors(metadata.Errors);
            }
            return await this.Run(input);
        }

        public abstract Metadata ValidateInput(TInput input);
        public abstract Task<OutputMessage<TOutput>> Run(TInput input);

    }
    
   
}
