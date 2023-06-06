using Newtonsoft.Json;
using System.Net.Http;
using SystemGateway.Dtos.Entities;

namespace SystemGateway.Providers
{
    public interface IDatabaseProvider
    {
        public Task<ICollection<User>> FindAllUsers();
        public Task<User?> FindUserById(string Email);
        public Task<ICollection<string>> FindPairedModulesByUserId(string Email);
        public Task<ICollection<Module>> FindAllModules();
        public Task<Module?> FindModuleById(string Id);
    }
}
