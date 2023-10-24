namespace SystemGateway
{
    public class ApplicationErrors
    {
        public static Error UnexpectedError => new Error(nameof(UnexpectedError), "Unexpected Error");
        public static Error FailedToCallDatabase => new Error(nameof(FailedToCallDatabase), "Call to Database API has failed");
        public static Error FailedToInitiateOperation => new Error(nameof(FailedToInitiateOperation), "Operation startup has failed");
        public static Error FailedToCommunicateWithSecurityManager => new Error(nameof(FailedToCommunicateWithSecurityManager), "Failed to Communicate with Security Manager");
        public static Error ModuleTemplateNotFound => new Error(nameof(ModuleTemplateNotFound), "Module Template Doesn't Exist");
        public static Error ModuleNotFound => new Error(nameof(ModuleNotFound), "Module Doesn't Exist");
        public static Error PatientNotFound => new Error(nameof(PatientNotFound), "Patient Doesn't Exist");
        public static Error TherapistNotFound => new Error(nameof(TherapistNotFound), "Therapist Doesn't Exist");
        public static Error NoPermissions => new Error(nameof(NoPermissions), "Current user doesn't have permission to access this data");
        public static Error UserNotFound => new Error(nameof(UserNotFound), "User was not found (Not Registered)");
        public static Error InvalidCredentials => new Error(nameof(InvalidCredentials), "Invalid Credentials");
        public static Error UserAlreadyExists => new Error(nameof(UserAlreadyExists), "There is already a user registered with that email");
    }
}
