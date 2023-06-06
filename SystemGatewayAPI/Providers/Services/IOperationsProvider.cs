using SystemGateway;
using SystemGateway.Dtos.Entities;

namespace SystemGateway.Providers
{
    public interface IOperationsProvider
    {
        public Task<bool> RegisterModule(Module module);
        public Task<bool> UpdateModule(string ModuleId, Module module);
        public Task<bool> DeleteModule(string ModuleId);


        public Task<bool> RegisterUser(User user); 
        public Task<bool> UpdateUser(string Email, User updatedUser);
        public Task<bool> DeleteUser(string Email);
        public Task<bool> RegisterConnection(ModuleConnection updatedUser);
        public Task<bool> DeleteConnection(Guid WebPlatformId, Guid ModuleId);
    }
}
