using Microsoft.AspNetCore.Mvc;
using Application.Services;
using System.Collections.Generic;
using System.Linq;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private static Dictionary<string, string> _refreshTokens = new Dictionary<string, string>();

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (IsValidUser(request.Username, request.Password))
            {
                var token = _authService.GenerateAccessToken(request.Username);
                var refreshToken = _authService.GenerateRefreshToken();

                // Store the refresh token
                if (_refreshTokens.ContainsKey(request.Username))
                {
                    _refreshTokens[request.Username] = refreshToken;
                }
                else
                {
                    _refreshTokens.Add(request.Username, refreshToken);
                }

                return Ok(new { Token = token, RefreshToken = refreshToken });
            }

            return Unauthorized();
        }

        [HttpPost("refresh-token")]
        public IActionResult Refresh([FromBody] RefreshRequest request)
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

        private bool IsValidUser(string username, string password)
        {
            // Replace this with actual validation logic
            return username == "admin" && password == "admin";
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
}
