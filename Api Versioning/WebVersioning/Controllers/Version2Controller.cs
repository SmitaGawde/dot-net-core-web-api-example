using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebVersioning.Controllers
{
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class Version2Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("This is version 2.0 of the API.");
        }
    }
}
