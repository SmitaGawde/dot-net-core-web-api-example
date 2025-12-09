using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JWTRefreshToken.Data;
using JWTRefreshToken.Models;

namespace JWTRefreshToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Services.TokenService _tokenService;
        private readonly AppDBContext _context;
        public AuthController(Services.TokenService tokenService, AppDBContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] Models.LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and password are required.");
            }
            Employee e = _context.employee.SingleOrDefault(e => e.Username == request.Username && e.Password == request.Password);
            if (e == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            // Here you would normally validate the user credentials against a database
            // For simplicity, we assume the credentials are valid
            var (token, refreshToken) = _tokenService.GenerateToken(request.Username, request.position);
            return Ok(new { Token = token, RefreshToken = refreshToken });
        }
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] Models.RefreshRequest request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest("Refresh token is required.");
            }
            var newToken = _tokenService.Refresh(request.RefreshToken);
            if (newToken == null)
            {
                return Unauthorized("Invalid refresh token.");
            }
            return Ok(new { Token = newToken });
        }
    }
}
