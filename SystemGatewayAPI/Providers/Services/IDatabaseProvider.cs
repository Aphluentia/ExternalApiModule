using DatabaseApi.Models.Entities;
using Newtonsoft.Json;
using System.Net.Http;

namespace SystemGateway.Providers
{
    public interface IDatabaseProvider
    {
        public Task<ICollection<Application>> FindAllApplications();
        public Task<Application?> FindApplicationById(string id);
        public Task<ICollection<Module>> FindAllModules();
        public Task<Module?> FindModuleById(string id);
        public Task<ICollection<Patient>> FindAllPatients();
        public Task<Patient?> FindPatientById(string email);
        public Task<ICollection<Therapist>> FindAllTherapists();
        public Task<Therapist?> FindTherapistById(string email);


    }
}
