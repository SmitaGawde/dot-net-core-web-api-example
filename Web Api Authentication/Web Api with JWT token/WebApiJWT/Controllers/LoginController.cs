using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiJWT.Data;
using WebApiJWT.Model;


namespace WebApiJWT.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly  AppDbContext db;
        private readonly IConfiguration configuration;
        public LoginController(AppDbContext _db, IConfiguration _configuration)
        {
            db = _db;
            configuration = _configuration;
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login (User user)
        {
            try
            {
                var usr = db.Users.FirstOrDefault(u => u.Id == user.Id && u.Password == user.Password);
                if (usr != null)
                {
                    var claims = new[] {
                    new  Claim (JwtRegisteredClaimNames.Sub , configuration["Jwt:Subject"]),
                    new  Claim (JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString () ),
                    new  Claim ("Id",user.Id.ToString ()  ),
                    new  Claim ("Username",user.Username.ToString ()  )
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                    var sigin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        issuer: configuration["Jwt:Issuer"],
                        audience: configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: sigin
                        );
                    string tokenvalue = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(new { Token = tokenvalue, user = user });
                    //return Ok(user); 
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }

        [Authorize]
        [HttpGet ]
        [Route("getUsers")]
        public IActionResult getUsers()
        {
            var lst = db.Users.Select (x => x).ToList ();
            return Ok(lst);
        }

    }
}
