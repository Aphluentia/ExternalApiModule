using ExternalAPI.Models.DatabaseDtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ExternalAPI.Helpers
{
    public class AuthenticationHelper
    {
        private static readonly string JWT_KEY = "JWT_KEY";
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
