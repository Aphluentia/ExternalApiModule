using SystemGatewayAPI.Dtos.Entities.Database;

namespace SystemGatewayAPI.Dtos.Entities.Secure
{
    public class SafePatient: SafeUser
    {
        public string ConditionName { get; set; }
        public DateTime ConditionAcquisitionDate { get; set; }
        public HashSet<string> AcceptedTherapists { get; set; }
        public HashSet<string> RequestedTherapists { get; set; }

        public static List<SafePatient> FromAll(ICollection<Patient> users)
        {
            var list = new List<SafePatient>();
            foreach (var user in users)
            {
                list.Add(FromPatient(user));
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
                Age = user.Age,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                ConditionName = user.ConditionName,
                ConditionAcquisitionDate = user.ConditionAcquisitionDate,
                AcceptedTherapists = user.AcceptedTherapists,
                RequestedTherapists = user.RequestedTherapists
            };
        }
    }
}
