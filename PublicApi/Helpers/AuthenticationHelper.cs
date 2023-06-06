
using System.Security.Cryptography;
using System.Text;

namespace PublicAPI.Helpers
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
            SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
       
    }
}
