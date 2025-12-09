using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _factory;
    private readonly TokenService _tokens;
    private readonly IConfiguration _config;

    public AccountController(IHttpClientFactory factory, TokenService tokens, IConfiguration config)
    {
        _factory = factory;
        _tokens = tokens;
        _config = config;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
    {
        var client = _factory.CreateClient();
        client.BaseAddress = new Uri(_config["Api:BaseUrl"] ?? "https://localhost:5001/");

        var payload = JsonSerializer.Serialize(new { Username = username, Password = password });
        var resp = await client.PostAsync("api/auth/login", new StringContent(payload, Encoding.UTF8, "application/json"));

        if (!resp.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Invalid credentials");
            return View();
        }

        var tokens = await resp.Content.ReadFromJsonAsync<TokenResponse>();
        if (tokens is null)
        {
            ModelState.AddModelError("", "Login failed");
            return View();
        }

        await _tokens.SetTokensAsync(tokens);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _tokens.ClearAsync();
        return RedirectToAction("Index", "Home");
    }
}
