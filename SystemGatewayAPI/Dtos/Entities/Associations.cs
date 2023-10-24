using SystemGatewayAPI.Dtos.Entities.Secure;

namespace SystemGatewayAPI.Dtos.Entities
{
    public class Associations<T>
    {

        public ICollection<T> Accepted { get; set; }
        public ICollection<T> Requested { get; set; }
        public ICollection<T> Available { get; set; }
        public ICollection<T> Pending { get; set; }
    }
}
