using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApptoConsumeJWTAPI.Models;
using Microsoft.AspNetCore.Authorization;


namespace WebApptoConsumeJWTAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _factory;

        private readonly ILogger<HomeController> _logger;

        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory factory, IConfiguration configuration)
        {
            _logger = logger;
            _factory = factory;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Getemployee()
        {
            var client = _factory.CreateClient("ApiClient");
            //client.BaseAddress = new Uri(_configuration["Api:BaseUrl"]!);
            var emp = await  client.GetFromJsonAsync<List<EmployeeDTo>>(_configuration["Api:BaseUrl"] + "api/Getemployee");
            return View(emp);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
