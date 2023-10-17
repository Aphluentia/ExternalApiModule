using DatabaseApi.Models.Entities;

namespace SystemGatewayAPI.Dtos.Entities
{
    public class SafePatient
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public int Age { get; set; }
        public string ConditionName { get; set; }
        public DateTime ConditionAcquisitionDate { get; set; }
        public string ProfilePicture { get; set; }
        public HashSet<string> AcceptedTherapists { get; set; }
        public HashSet<string> RequestedTherapists { get; set; }

        public static List<SafePatient> FromAll(ICollection<Patient> users)
        {
            var list = new List<SafePatient>();
            foreach (var user in users)
            {
                list.Add(SafePatient.FromPatient(user));
            }
            return list;
        }
        public static SafePatient FromPatient(Patient user)
        {
            return new SafePatient
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                CountryCode = user.CountryCode,
                Age = user.Age,
                ConditionName = user.ConditionName,
                ConditionAcquisitionDate = user.ConditionAcquisitionDate,
                ProfilePicture = user.ProfilePicture,
                AcceptedTherapists = user.AcceptedTherapists,
                RequestedTherapists = user.RequestedTherapists
            };
        }
    }
}
