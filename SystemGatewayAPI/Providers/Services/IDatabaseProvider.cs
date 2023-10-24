using Newtonsoft.Json;
using System.Net.Http;
using SystemGatewayAPI.Dtos.Entities.Database;
using SystemGatewayAPI.Dtos.Entities.Secure;

namespace SystemGateway.Providers
{
    public interface IDatabaseProvider
    {
        public Task<ICollection<Application>> FindAllApplications();
        public Task<Application?> FindApplicationById(string id);
        public Task<ICollection<Module>> FindAllModules();
        public Task<Module?> FindModuleById(Guid id);
        public Task<ICollection<SafePatient>> FindAllPatients();
        public Task<Patient?> FindPatientById(string email);
        public Task<ICollection<Module>> FindAllPatientModules(string email);
        public Task<Module> FindPatientModuleById(string email, Guid ModuleId);
        public Task<ICollection<SafeTherapist>> FindAllTherapists();
        public Task<Therapist?> FindTherapistById(string email);


    }
}
