
using System.Security.Cryptography;
using System.Text;

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
       
    }
}
