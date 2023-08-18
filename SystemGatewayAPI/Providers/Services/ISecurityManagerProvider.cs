using SystemGateway.Dtos.SecurityManager;

namespace SystemGateway.Providers
{
    public interface ISecurityManagerProvider
    {
        public Task<string> GenerateSession(SecurityDataDto securityData);
        public Task<bool> KeepAlive(string Token);
        public Task<bool> ValidateSession(string Token);
        public Task<SecurityDataDto> GetTokenData(string Token);

    }
}
