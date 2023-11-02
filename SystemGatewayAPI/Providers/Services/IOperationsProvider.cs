using SystemGatewayAPI.Dtos.Entities;
using SystemGatewayAPI.Dtos.Entities.Database;

namespace SystemGateway.Providers
{
    public interface IOperationsProvider
    {
        public Task<ActionResponse> RegisterApplication(Application application); //REGISTER_APPLICATION
        public Task<ActionResponse> AddApplicationVersion(string ApplicationName, ModuleVersion version); //ADD_APPLICATION_VERSION
        public Task<ActionResponse> UpdateApplicationVersion(string ApplicationName, string VersionId, ModuleVersion version); //UPDATE_APPLICATION_VERSION,
        public Task<ActionResponse> DeleteApplicationVersion(string ApplicationName, string VersionId); //DELETE_APPLICATION_VERSION,
        public Task<ActionResponse> DeleteApplication(string ApplicationName); //DELETE_APPLICATION,



        public Task<ActionResponse> RegisterModule(Module Module); //CREATE_MODULE
        public Task<ActionResponse> UpdateModule(string ModuleId, Module module); //UPDATE_MODULE,
        public Task<ActionResponse> ModuleAddNewContext(string ModuleId, string ProfileName);
        public Task<ActionResponse> ModuleDeleteContext(string ModuleId, string ProfileName);
        public Task<ActionResponse> UpdateModuleToVersion(string ModuleId, string VersionId); //UPDATE_MODULE_TO_VERSION
        public Task<ActionResponse> DeleteModule(string ModuleId); //DELETE_MODULE,


        public Task<ActionResponse> RegisterPatient(Patient patient); //CREATE_PATIENT
        public Task<ActionResponse> UpdatePatient(string Email, Patient patient); //UPDATE_PATIENT,
        public Task<ActionResponse> DeletePatient(string Email); //DELETE_PATIENT,
        public Task<ActionResponse> AddExistingModuleToPatient(string Email, string ModuleId); //PATIENT_ADD_EXISTING_MODULE,
        public Task<ActionResponse> AddNewModuleToPatient(string Email, Module Module); //PATIENT_NEW_MODULE
        public Task<ActionResponse> PatientAddNewModuleContext(string Email, string ModuleId, string ProfileName);
        public Task<ActionResponse> PatientDeleteModuleContext(string Email, string ModuleId, string ProfileName);
        public Task<ActionResponse> UpdatePatientModule(string Email, string ModuleId, Module Module); //UPDATE_PATIENT_MODULE
        public Task<ActionResponse> UpdatePatientModuleToVersion(string Email, string ModuleId, string VersionId); //UPDATE_PATIENT_MODULE_VERSION
        public Task<ActionResponse> DeletePatientModule(string Email, string ModuleId); //DELETE_PATIENT_MODULE
        public Task<ActionResponse> PatientRequestTherapist(string Email, string TherapistEmail); //PATIENT_REQUEST_THERAPIST
        public Task<ActionResponse> PatientRejectTherapist(string Email, string TherapistEmail); //PATIENT_REJECT_THERAPIST


        
        public Task<ActionResponse> RegisterTherapist(Therapist therapist); //CREATE_THERAPIST,
        public Task<ActionResponse> UpdateTherapist(string Email, Therapist therapist); //UPDATE_THERAPIST,
        public Task<ActionResponse> DeleteTherapist(string Email); //DELETE_THERAPIST,
        public Task<ActionResponse> TherapistAcceptPatient(string Email, string PatientEmail); //THERAPIST_ACCEPT_PATIENT,
        public Task<ActionResponse> TherapistRejectPatient(string Email, string PatientEmail); //THERAPIST_REJECT_PATIENT,



    }
}
