using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

builder.Services.AddHttpContextAccessor();

// Configure a named HttpClient for the API with a DelegatingHandler that injects/refreshes tokens
builder.Services.AddTransient<TokenHandler>();
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Api:BaseUrl"] ?? "https://localhost:5001/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
}).AddHttpMessageHandler<TokenHandler>();

builder.Services.AddSingleton<TokenService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// ---------------- Services & Handler ----------------
public record LoginRequest(string Username, string Password);
public record TokenResponse(string AccessToken, string RefreshToken, DateTime ExpiresAtUtc);

public class TokenService
{
    private readonly IHttpContextAccessor _ctx;
    public TokenService(IHttpContextAccessor ctx) => _ctx = ctx;

    public async Task SetTokensAsync(TokenResponse tokens, bool signIn = true)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "api-user"),
            new Claim("access_token", tokens.AccessToken),
            new Claim("refresh_token", tokens.RefreshToken),
            new Claim("access_expires_utc", tokens.ExpiresAtUtc.ToUniversalTime().ToString("o"))
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        if (signIn)
        {
            await _ctx.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8) });
        }
        else
        {
            // Update existing cookie
            await _ctx.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }

    public string? GetAccessToken() => _ctx.HttpContext?.User.FindFirstValue("access_token");
    public string? GetRefreshToken() => _ctx.HttpContext?.User.FindFirstValue("refresh_token");
    public DateTimeOffset? GetAccessExpiryUtc()
    {
        var s = _ctx.HttpContext?.User.FindFirst("access_expires_utc")?.Value;
        return s is null ? null : DateTimeOffset.Parse(s);
    }

    public async Task ClearAsync()
    {
        await _ctx.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}

public class TokenHandler : DelegatingHandler
{
    private readonly TokenService _tokens;
    private readonly IHttpClientFactory _factory;
    private readonly IConfiguration _config;

    public TokenHandler(TokenService tokens, IHttpClientFactory factory, IConfiguration config)
    {
        _tokens = tokens;
        _factory = factory;
        _config = config;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // attach access token
        var access = _tokens.GetAccessToken();
        if (!string.IsNullOrEmpty(access))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            // try refresh
            var refresh = _tokens.GetRefreshToken();
            if (!string.IsNullOrEmpty(refresh))
            {
                var refreshReq = new HttpRequestMessage(HttpMethod.Post, "api/auth/refresh")
                {
                    Content = new StringContent(JsonSerializer.Serialize(new { RefreshToken = refresh }), Encoding.UTF8, "application/json")
                };

                // Use a plain client (avoid recursion with handler)
                var plain = _factory.CreateClient();
                plain.BaseAddress = new Uri(_config["Api:BaseUrl"] ?? "https://localhost:5001/");
                var refreshResp = await plain.SendAsync(refreshReq, cancellationToken);
                if (refreshResp.IsSuccessStatusCode)
                {
                    var tokens = await refreshResp.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: cancellationToken);
                    if (tokens is not null)
                    {
                        await _tokens.SetTokensAsync(tokens, signIn: false);
                        // retry original request with new token
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
                        response.Dispose();
                        response = await base.SendAsync(request, cancellationToken);
                    }
                }
            }
        }

        return response;
    }
}
