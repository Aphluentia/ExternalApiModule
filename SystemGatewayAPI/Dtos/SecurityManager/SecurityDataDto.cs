using SystemGateway.Dtos.Enum;

namespace SystemGateway.Dtos.SecurityManager
{
    public class SecurityDataDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public Permission PermissionLevel { get; set; }
        public Guid WebPlatformId { get; set; }
        public static SecurityDataDto Empty()
        {
            return new SecurityDataDto
            {
                UserId = "",
                UserName = "",
                PermissionLevel = Permission.Client,
                WebPlatformId = Guid.Empty
            };
        }
    }
}
