using JWTTokenWebApi.Context;
using JWTTokenWebApi.Interfaces;
using JWTTokenWebApi.Models;
using JWTTokenWebApi.Request_Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JWTTokenWebApi.Sefvices
{
    public class AuthService : IAuthService
    {
        private readonly JwtContext Jwtcontext;
        private readonly IConfiguration configuration;
        public AuthService(JwtContext _context ,   IConfiguration _configuration)
        {
            Jwtcontext = _context;
            configuration = _configuration;
        }
        public user Adduser(user _user)
        {
            var Addedusers = Jwtcontext.Add(_user);
            Jwtcontext.SaveChanges();
            return Addedusers.Entity;
        }

        public string login(LoginRequest loginRequest)
        {
            if (loginRequest.username != null && loginRequest.password != null)
            {
                var user = Jwtcontext.Users.FirstOrDefault(x => x.Username == loginRequest.username && x.password == loginRequest.password);
                if (user != null)
                {
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("Username", user.Username),
                        new Claim("Id", user.id.ToString()),
                        new Claim("Email", user.email)
                    };
                    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        issuer: configuration["Jwt:Issuer"],
                        audience: configuration["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: creds
                    );
                    return new JwtSecurityTokenHandler().WriteToken(token) + "\nLogin Successful";

                    //return "Login Successful";
                }
                else
                {
                    return "Invalid Credentials";
                }
            }
            else
            {
                return "Username or Password cannot be null";
            }   
        }
    }
}
