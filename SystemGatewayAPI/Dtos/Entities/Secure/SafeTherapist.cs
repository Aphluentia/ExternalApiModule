using SystemGatewayAPI.Dtos.Entities.Database;

namespace SystemGatewayAPI.Dtos.Entities.Secure
{
    public class SafeTherapist: SafeUser
    {
        public string Credentials { get; set; }
        public string Description { get; set; }
        public HashSet<string>? AcceptedPatients { get; set; }
        public HashSet<string>? RequestedPatients { get; set; }

        public static List<SafeTherapist> FromAll(ICollection<Therapist> users)
        {
            var list = new List<SafeTherapist>();
            foreach (var user in users)
            {
                list.Add(FromTherapist(user));
            }
            return list;
        }
        public static SafeTherapist FromTherapist(Therapist user)
        {
            return new SafeTherapist
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                Credentials = user.Credentials,
                Description = user.Description,
                AcceptedPatients = user.AcceptedPatients,
                RequestedPatients = user.RequestedPatients
            };
        }
    }
}
