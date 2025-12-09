using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace JWTRefreshToken.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private static Dictionary<string, string> refreshTokens = new();
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public (string token, string refreshToken) GenerateToken(string username, string position, int expirationMinutes = 10)
        {
            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key));
            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, username),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, position)
                }),
                Expires = DateTime.UtcNow.AddSeconds(expirationMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = credentials
            };
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = Guid.NewGuid().ToString();
            refreshTokens[refreshToken] = username;
            return  (tokenHandler.WriteToken(token),  refreshToken);
        }

        public string Refresh(string refreshToken)
        {
            if (refreshTokens.ContainsKey(refreshToken))
            {
                var username = refreshTokens[refreshToken];
                var position = refreshTokens[refreshToken];
                return GenerateToken(username,position).token;
            }
            return null;
        }
    }
}
