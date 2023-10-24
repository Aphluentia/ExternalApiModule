
using System.Security.Cryptography;
using System.Text;
using SystemGatewayAPI.Dtos.Entities.Database;

namespace SystemGateway.Helpers
{
    public class AuthenticationHelper
    {
        public static bool AuthenticateUser(string UserPassword, string ProvidedPassword)
        {
            if (string.Equals(HashPassword(ProvidedPassword), UserPassword))
                return true;
            return false;
        }
        public static string HashPassword(string password)
        {
            SHA512 sha512 = SHA512.Create();
            byte[] hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
        public static string ValidateUser(User user)
        {
            if (string.IsNullOrEmpty(user.Email)) return "Email is Required";
            if (string.IsNullOrEmpty(user.Password)) return "Password is Required";
            if (string.IsNullOrEmpty(user.FirstName)) return "First Name is Required";
            if (string.IsNullOrEmpty(user.LastName)) return "Last Name is Required";
            return "";
        }
       
    }
}
