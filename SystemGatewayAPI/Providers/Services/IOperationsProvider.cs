using DatabaseApi.Models.Entities;

namespace SystemGateway.Providers
{
    public interface IOperationsProvider
    {
        public Task<bool> RegisterApplication(Application module); //REGISTER_APPLICATION
        public Task<bool> AddApplicationVersion(string ApplicationId, ModuleVersion module); //ADD_APPLICATION_VERSION
        public Task<bool> UpdateApplicationVersion(string ModuleId, string VersionId, ModuleVersion module); //UPDATE_MODULE,
        public Task<bool> DeleteApplicationVersion(string ModuleId, string VersionId); //DELETE_APPLICATION_VERSION,
        public Task<bool> DeleteApplication(string ModuleId); //DELETE_APPLICATION,



        public Task<bool> RegisterModule(Module module); //CREATE_MODULE
        public Task<bool> UpdateModule(string ModuleId, Module module); //UPDATE_MODULE,
        public Task<bool> UpdateModuleToVersion(string ModuleId, string VersionId); //UPDATE_MODULE_TO_VERSION,
        public Task<bool> UpdateModuleVersion(string ModuleId, ModuleVersion version); //UPDATE_MODULE_VERSION,
        public Task<bool> DeleteModule(string ModuleId); //DELETE_MODULE,


        public Task<bool> RegisterPatient(Patient patient); //CREATE_PATIENT
        public Task<bool> UpdatePatient(string Email, Patient patient); //UPDATE_PATIENT,
        public Task<bool> DeletePatient(string Email); //DELETE_PATIENT,
        public Task<bool> AddExistingModuleToPatient(string Email, string ModuleId); //ADD_PATIENT_EXISTING_MODULE,
        public Task<bool> AddNewModuleToPatient(string Email, Module Module); //ADD_PATIENT_NEW_MODULE
        public Task<bool> UpdatePatientModule(string Email, string ModuleId, Module Module); //UPDATE_PATIENT_MODULE
        public Task<bool> UpdatePatientModuleToVersion(string Email, string ModuleId, string VersionId); //UPDATE_PATIENT_MODULE
        public Task<bool> DeletePatientModule(string Email, string ModuleId); //DELETE_PATIENT_MODULE
        public Task<bool> PatientRequestTherapist(string Email, string TherapistEmail); //PATIENT_REQUEST_THERAPIST
        public Task<bool> PatientRejectTherapist(string Email, string TherapistEmail); //PATIENT_REJECT_THERAPIST


        
        public Task<bool> RegisterTherapist(Therapist therapist); //CREATE_THERAPIST,
        public Task<bool> UpdateTherapist(string Email, Therapist therapist); //UPDATE_THERAPIST,
        public Task<bool> DeleteTherapist(string Email); //DELETE_THERAPIST,
        public Task<bool> TherapistAcceptPatient(string Email, string PatientEmail); //THERAPIST_ACCEPT_PATIENT,
        public Task<bool> TherapistRejectPatient(string Email, string PatientEmail); //THERAPIST_REJECT_PATIENT,



    }
}
