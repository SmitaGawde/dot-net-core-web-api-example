using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebApptoConsumeJWTAPI.Services
{
    public class TokenService
    {
        private readonly IHttpContextAccessor _http;
        public TokenService(IHttpContextAccessor http)
        {
            _http = http;
        }

        public async Task SetTokensAsync (string token,string refreshToken)
        {
            var claims = new List<Claim>
            {
                new Claim("access_token", token),
                new Claim("refresh_token", refreshToken)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _http.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        }

        public string? GetAccessToken() => _http.HttpContext?.User.FindFirstValue("access_token");
        public string? GetRefreshToken() => _http.HttpContext?.User.FindFirstValue("refresh_token");

        public async Task ClearAsync() => await _http.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    }
}
