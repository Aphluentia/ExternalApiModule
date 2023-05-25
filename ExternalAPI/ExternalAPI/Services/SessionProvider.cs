
using ExternalAPI.Configurations;
using ExternalAPI.Models.DatabaseDtos;
using ExternalAPI.Models.Enums;
using ExternalAPI.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ExternalApi.Services
{

    public class SessionProvider : ISessionProvider
    {
        private SessionConfigSection _sessionConfigSection;
        
        public SessionProvider( IOptions<SessionConfigSection> sessionConfig)
        {
            _sessionConfigSection = sessionConfig.Value;
            
        }



        public string CreateToken(UserDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_sessionConfigSection.JWTSecret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
           
            var claims = new List<Claim>
            {
                new Claim("UserEmail", user.Email),
                new Claim("PermissionLevel", user.PermissionLevel.ToString()),
                new Claim("WebPlatformId", user.WebPlatformId.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer:_sessionConfigSection.Issuer,
                audience: _sessionConfigSection.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_sessionConfigSection.SessionValidityInMinutes),
                signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }



        

        public string KeepAlive(string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_sessionConfigSection.JWTSecret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();
            var decodedToken = tokenHandler.ReadJwtToken(token);

            var claims = decodedToken.Claims;


            var newToken = new JwtSecurityToken(
                issuer: _sessionConfigSection.Issuer,
                audience: _sessionConfigSection.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_sessionConfigSection.SessionValidityInMinutes),
                signingCredentials: signingCredentials);


            return tokenHandler.WriteToken(newToken); 
        }

        public (string?, PermissionLevel?, string?) GetClaims(string Token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_sessionConfigSection.JWTSecret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();
            var decodedToken = tokenHandler.ReadJwtToken(Token);

            var claims = decodedToken.Claims;
            return (claims.FirstOrDefault(c => c.Type == "UserEmail")?.Value, (PermissionLevel)Enum.Parse(typeof(PermissionLevel), claims.FirstOrDefault(c => c.Type == "PermissionLevel")?.Value), claims.FirstOrDefault(c => c.Type == "WebPlatformId")?.Value);
        }
        public bool ValidateToken(string Token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_sessionConfigSection.JWTSecret)),
                ValidateIssuer = true,
                ValidIssuer = _sessionConfigSection.Issuer,
                ValidateAudience = true,
                ValidAudience = _sessionConfigSection.Audience
            };

            try
            {
                tokenHandler.ValidateToken(Token, validationParameters, out var validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
