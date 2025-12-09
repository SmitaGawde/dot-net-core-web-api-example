using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
//using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiJWT.Data;
using WebApiJWT.Models;

namespace WebApiJWT.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext Db;
        private readonly IConfiguration configuration;
        public UserController(AppDbContext _Db, IConfiguration _configuration)
        {
            Db = _Db;
            configuration = _configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(User user)
        {
            var usr = Db.users.FirstOrDefault(u => u.Id == user.Id && u.Password == user.Password);
            if (user != null)
            {
                var claims = new[] {
                    new  Claim (JwtRegisteredClaimNames.Sub ,configuration["Jwt:Subject"] ),
                    new Claim (JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                    new Claim ("Username",usr.Username ),
                    new Claim ("Id",usr.Id.ToString ()  )
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: cred
                    );
                string tokenvalue = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { Token = tokenvalue, User = user });


            }
            return NoContent();
        }

        [HttpGet]
        [Authorize]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            var lst = Db.users.ToList();
            return Ok(lst);
        }
    }
}
