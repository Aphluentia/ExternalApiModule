using ExternalAPI.Models.Entities;
using System.Runtime.CompilerServices;

namespace ExternalAPI.Models.Dtos
{
    public class OutputMessage<T>
    {
        public Metadata Metadata { get; set; }
        public T Data { get; set; }

        public static OutputMessage<T> GetOutputMessage(T? obj)
        {
            return new OutputMessage<T>
            {
                Metadata = new Models.Metadata(),
                Data = obj
            };
        }
        public static OutputMessage<T> GetOutputMessage()
        {
            return new OutputMessage<T>
            {
                Metadata = new Models.Metadata()
            };
        }
     

        public OutputMessage<T> AddError(Error error)
        {
            this.Metadata.Errors.Add(error);
            return this;
        }
    }

    
}
