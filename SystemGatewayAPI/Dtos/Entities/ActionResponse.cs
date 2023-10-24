namespace SystemGatewayAPI.Dtos.Entities
{
    public class ActionResponse
    {
        public Guid TaskId { get; set; }
        public System.Net.HttpStatusCode Code { get; set; }
        public string Message { get; set; }
    }
}
