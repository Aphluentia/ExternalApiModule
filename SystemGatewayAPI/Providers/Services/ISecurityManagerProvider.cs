using SystemGateway.Dtos.SecurityManager;
using SystemGatewayAPI.Dtos.SecurityManager;

namespace SystemGateway.Providers
{
    public interface ISecurityManagerProvider
    {
        public Task<string> GenerateSession(SecurityDataDto securityData);
        public Task<bool> KeepAlive(string Token);
        public Task<bool> ValidateSession(string Token);
        public Task<SecurityDataDto> FetchTokenData(string Token);
        public Task<bool> AddModuleSnapshot(string Token, ModuleSnapshot moduleSnapshot);
        public Task<bool> UpdateModuleSnapshot(string Token, Guid ModuleId, ModuleSnapshot moduleSnapshot);
        public Task<bool> DeleteModuleSnapshot(string Token, Guid ModuleId);

    }
}
