using JWTTokenWebApi.Interfaces;
using JWTTokenWebApi.Models;
using JWTTokenWebApi.Request_Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWTTokenWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        // POST api/<AuthController>
        [HttpPost]
        public string Login([FromBody] LoginRequest loginmodel)
        {
            var result = _authService.login(loginmodel);
            return result;
        }

        // PUT api/<AuthController>/5
        [HttpPost]
        public user AddUser(int id, [FromBody] user _user)
        {
            var addedUser = _authService.Adduser(_user);    
            return addedUser;
        }

 
    }
}
