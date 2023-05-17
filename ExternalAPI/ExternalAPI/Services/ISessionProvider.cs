using ExternalAPI.Models.DatabaseDtos;
using ExternalAPI.Models.Enums;

namespace ExternalAPI.Services
{
    public interface ISessionProvider
    {

        public bool ValidateToken(string Token);
        public string CreateToken(UserDto user);
        public string KeepAlive(string token);
        public (string?, PermissionLevel?, Guid?) GetClaims(string Token);
    }
}
