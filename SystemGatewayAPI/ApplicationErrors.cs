namespace SystemGateway
{
    public class ApplicationErrors
    {
        public static Error UnexpectedError => new Error(nameof(UnexpectedError), "Unexpected Error");
        public static Error FailedToCallDatabase => new Error(nameof(FailedToCallDatabase), "Call to Database API has failed");
        public static Error FailedToInitiateOperation => new Error(nameof(FailedToInitiateOperation), "Operation startup has failed");
        public static Error FailedToCommunicateWithSecurityManager => new Error(nameof(FailedToCommunicateWithSecurityManager), "Failed to Communicate with Security Manager");

    }
}
