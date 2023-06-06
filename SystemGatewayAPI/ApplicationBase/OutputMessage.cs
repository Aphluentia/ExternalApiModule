namespace SystemGateway
{
    public class OutputMessage<T>
    {
        public Metadata Metadata { get; set; }
        public T Data { get; set; }

        public static OutputMessage<T> GetOutputMessage(T? obj)
        {
            return new OutputMessage<T>
            {
                Metadata = new Metadata(),
                Data = obj
            };
        }
        public static OutputMessage<T> GetOutputMessage()
        {
            return new OutputMessage<T>
            {
                Metadata = new Metadata()
            };
        }


        public OutputMessage<T> AddError(Error error)
        {
            this.Metadata.Errors.Add(error);
            return this;
        }
        public OutputMessage<T> AddManyErrors(ICollection<Error> error)
        {
            foreach(Error e in error) 
                this.Metadata.Errors.Add(e);
            return this;
        }
    }

}
