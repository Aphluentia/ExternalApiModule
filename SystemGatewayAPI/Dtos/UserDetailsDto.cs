using DatabaseApi.Models.Entities;
using SystemGateway.Dtos.Enum;

namespace SystemGatewayAPI.Dtos
{
    public class UserDetailsDto
    {
        public string Email { get; set; }
        public UserType UserType { get; set; }
        public string FullName { get; set; }
        public static UserDetailsDto FromTherapist(Therapist therapist)
        {
            return new UserDetailsDto
            {
                Email = therapist.Email,
                UserType = UserType.Therapist,
                FullName = $"{therapist.FirstName} {therapist.LastName}"
            };
        }
        public static UserDetailsDto FromPatient(Patient patient)
        {
            return new UserDetailsDto
            {
                Email = patient.Email,
                UserType = UserType.Patient,
                FullName = $"{patient.FirstName} {patient.LastName}"
            };
        }
    }
   
}
