namespace SystemGatewayAPI.Dtos.Entities.Database
{
    public class Therapist: User
    {
        public string Credentials { get; set; }
        public string Description { get; set; }
        public HashSet<string>? AcceptedPatients { get; set; } = new HashSet<string>();
        public HashSet<string>? RequestedPatients { get; set; } = new HashSet<string>();

    }
}
