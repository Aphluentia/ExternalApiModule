namespace SystemGatewayAPI.Dtos.Entities
{
    public class TherapistPatients
    {
        public ICollection<SafePatient> Accepted { get; set; }
        public ICollection<SafePatient> Requested { get; set; }
        public ICollection<SafePatient> Available { get; set; }
        public ICollection<SafePatient> Pending { get; set; }
    }
}
