using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using WebApptoConsumeJWTAPI.Services;
//using WebApptoConsumeJWTAPI.Services;

namespace WebApptoConsumeJWTAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokens;
        private readonly IHttpClientFactory _factory;

        public AccountController(IConfiguration configuration, TokenService tokens, IHttpClientFactory factory)
        {
            _configuration = configuration;
            _tokens = tokens;
            _factory = factory;
        }

        [HttpGet]
        public IActionResult Login()
        {            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Username and password are required.";
                return View();
            }
            var client = _factory.CreateClient();

            client.BaseAddress = new Uri(_configuration["Api:BaseUrl"]!);
            //var payload = new { username = username, password = password };
            var payload = new { username = username, password = password, position = "Manager" };
            var resp = await client.PostAsync("api/Auth/Login",
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));
            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid credentials or API error.";
                return View();
            }
            var obj = await  resp.Content.ReadFromJsonAsync<JsonElement>();
            var token = obj.GetProperty("token").GetString();
            var refreshToken = obj.GetProperty("refreshToken").GetString();
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(refreshToken))
            {
                ViewBag.Error = "Invalid token data from API.";
                return View();
            }
            await _tokens.SetTokensAsync(token, refreshToken);
            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _tokens.ClearAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
