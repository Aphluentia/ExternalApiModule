namespace PublicAPI.Models.Entities
{
    public class Error
    {
        public string Code { get; private set; }

        public string Message { get; private set; }

        public Error(string errorCode, string errorMessage)
        {
            Code = errorCode;
            Message = errorMessage;
        }
    }
}
