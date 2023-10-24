using SystemGateway.Dtos.Enum;
using SystemGateway.Dtos.SecurityManager;
using SystemGatewayAPI.Dtos.Entities.Database;

namespace SystemGatewayAPI.Dtos
{
    public class SessionData: SecurityDataDto
    {
        public string FullName { get; set; }
        public static SessionData MapFromUser(User therapist, SecurityDataDto security)
        {
            return new SessionData
            {
                Email = therapist.Email,
                UserType = security.UserType,
                FullName = $"{therapist.FirstName} {therapist.LastName}",
                Expires = security.Expires,
                ModuleSnapshots = security.ModuleSnapshots
            };
        }
       
    }
   
}
