using PublicAPI.Models.DatabaseDtos;
using PublicAPI.Models.Enums;

namespace PublicAPI.Services
{
    public interface ISessionProvider
    {

        public bool ValidateToken(string Token);
        public string CreateToken(UserDto user);
        public string KeepAlive(string token);
        public (string?, PermissionLevel?, string?) GetClaims(string Token);
    }
}
