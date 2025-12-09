using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebVersioning.Controllers
{
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class Version1Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("This is version 1.0 of the API.");
        }
    }
}
