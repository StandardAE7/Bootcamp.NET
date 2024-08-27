using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using Application.Models;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Services
{
    public class AuthService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IDistributedCache _cache;
        private readonly HashSet<string> _tokenBlacklist;
        private readonly List<User> _users;

        public AuthService(string secretKey, string issuer, string audience, IDistributedCache cache, HashSet<string> tokenBlacklist)
        {
            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
            _cache = cache;
            _tokenBlacklist = tokenBlacklist;
            _users = new List<User>
        {
            new User { Username = "admin", IsLogout = true },
            new User { Username = "user", IsLogout = true }
        };
        }

        public bool IsValidUser(string username, string password)
        {
            // Replace this with actual validation logic
            return (username == "admin" && password == "admin") || (username == "user" && password == "user");
        }

        public User GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public string GenerateAccessToken(string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            // Assign roles based on the username
            if (string.Equals(username, "admin", StringComparison.OrdinalIgnoreCase))
            {
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
            }
            else if (string.Equals(username, "user", StringComparison.OrdinalIgnoreCase))
            {
                claims.Add(new Claim(ClaimTypes.Role, "user"));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public string GenerateAccessTokenFromRefreshToken(string refreshToken, out string username)
        {
            var principal = GetPrincipalFromToken(refreshToken);
            username = principal.Identity.Name;

            return GenerateAccessToken(username);
        }

        public void AddToBlacklist(string token)
        {
            _cache.SetString(token, "blacklisted");
        }

        public bool IsTokenBlacklisted(string token)
        {
            var value = _cache.GetString(token);
            return value != null && value == "blacklisted";
        }

        public bool ValidateToken(string token)
        {
            if (IsTokenBlacklisted(token))
            {
                throw new SecurityTokenException("This token is no longer valid.");
            }

            var principal = GetPrincipalFromToken(token);
            return principal != null;
        }
        public bool IsUserLoggedOut(string username)
        {
            var user = GetUserByUsername(username);
            return user != null && user.IsLogout;
        }
    }
}
