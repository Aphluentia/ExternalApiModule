namespace SystemGatewayAPI.Dtos.Entities
{
    public class PatientTherapist
    {

        public ICollection<SafeTherapist> Accepted { get; set; }
        public ICollection<SafeTherapist> Requested { get; set; }
        public ICollection<SafeTherapist> Available { get; set; }
        public ICollection<SafeTherapist> Pending { get; set; }
    }
}
