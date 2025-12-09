using BasicAuthApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace BasicAuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [BasicAuthentication]
    public class SecureController : ApiController
    {
        [HttpGet("data")]
        //[System.Web.Http.Route("api/secure/data")]
        public IHttpActionResult GetSecureData()
        {
            var username = Thread.CurrentPrincipal.Identity.Name;
            return (IHttpActionResult)Ok("Hello " + username + ", your data is secure!");
        }
    }
}
