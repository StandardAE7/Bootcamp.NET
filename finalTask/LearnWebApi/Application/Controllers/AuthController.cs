using Microsoft.AspNetCore.Mvc;
using Application.Services;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IDistributedCache _cache;
        private static Dictionary<string, string> _refreshTokens = new Dictionary<string, string>();

        public AuthController(AuthService authService, IDistributedCache cache)
        {
            _authService = authService;
            _cache = cache;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (_authService.IsValidUser(request.Username, request.Password))
            {
                var user = _authService.GetUserByUsername(request.Username);
                if (user != null)
                {
                    user.IsLogout = false;

                    var token = _authService.GenerateAccessToken(request.Username);
                    var refreshToken = _authService.GenerateRefreshToken();

                    // Cache the refresh token
                    await _cache.SetStringAsync(request.Username, refreshToken);

                    return Ok(new { Token = token, RefreshToken = refreshToken });
                }
            }

            return Unauthorized();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            // Check if the refresh token exists
            if (!_refreshTokens.ContainsValue(request.RefreshToken))
            {
                return Unauthorized();
            }

            // Find the username associated with the refresh token
            var username = _refreshTokens.FirstOrDefault(x => x.Value == request.RefreshToken).Key;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized();
            }

            // Generate new tokens
            var newJwtToken = _authService.GenerateAccessToken(username);
            var newRefreshToken = _authService.GenerateRefreshToken();

            // Update the refresh token
            _refreshTokens[username] = newRefreshToken;

            return Ok(new { Token = newJwtToken, RefreshToken = newRefreshToken });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            var principal = _authService.GetPrincipalFromToken(request.AccessToken);
            var username = principal.Identity.Name;

            var user = _authService.GetUserByUsername(username);
            if (user != null)
            {
                user.IsLogout = true;

                // Cache the access token to blacklist
                await _cache.SetStringAsync(request.AccessToken, "blacklisted");
            }

            return Ok(new { Message = "Logged out successfully" });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RefreshRequest
    {
        public string RefreshToken { get; set; }
    }
    public class LogoutRequest
    {
        public string AccessToken { get; set; }
    }
}
