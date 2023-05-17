using ExternalAPI.Models.Entities;

namespace ExternalAPI
{
    public class ApplicationErrors
    {

        public static Error UnexpectedError => new Error(nameof(UnexpectedError), "Unexpected Error");
        public static Error UserAuthenticationFailed => new Error(nameof(UserAuthenticationFailed), "Failed to Authenticate User");

        public static Error FailedToCallDatabase => new Error(nameof(FailedToCallDatabase), "Call to Database API has failed");
        public static Error EmailIsRequired => new Error(nameof(EmailIsRequired), "User email is required for authenticatin");

        public static Error PasswordIsRequired => new Error(nameof(PasswordIsRequired), "Password is required for authentication");
        public static Error UserNotFound => new Error(nameof(UserNotFound), "User was not found");
        public static Error FailedToCreateUser => new Error(nameof(FailedToCreateUser), "Failed to create new User");
        public static Error FailedToCreateConnection => new Error(nameof(FailedToCreateConnection), "Failed to publish new Module Connection");
    }
}
