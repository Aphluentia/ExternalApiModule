using DatabaseApi.Models.Entities;

namespace SystemGatewayAPI.Dtos.Entities
{
    public class SafeTherapist
    {

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Credentials { get; set; }
        public string Description { get; set; }
        public string ProfilePicture { get; set; }
        public HashSet<string> PatientsAccepted { get; set; }
        public HashSet<string> PatientRequests { get; set; }

        public static List<SafeTherapist> FromAll(ICollection<Therapist> users)
        {
            var list = new List<SafeTherapist>();
            foreach (var user in users)
            {
                list.Add(SafeTherapist.FromTherapist(user));
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
                Credentials = user.Credentials,
                Description = user.Description,
                Age = user.Age,
                ProfilePicture = user.ProfilePicture,
                PatientsAccepted = user.PatientsAccepted,
                PatientRequests = user.PatientRequests
            };
        }
    }
}
