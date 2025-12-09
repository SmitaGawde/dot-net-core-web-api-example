using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Simple in-memory refresh token store (demo only)
var refreshStore = new Dictionary<string, string>(); // refreshToken -> username

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "DemoApi",
            ValidAudience = "DemoClient",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super-secret-key-change-me-please-12345"))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

// Models
record LoginRequest(string Username, string Password);
record TokenResponse(string AccessToken, string RefreshToken, DateTime ExpiresAtUtc);
record RefreshRequest(string RefreshToken);
record ProductDto(int Id, string Name);

// Token helpers
string CreateAccessToken(string username)
{
    var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, username),
        new Claim(ClaimTypes.Name, username)
    };
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super-secret-key-change-me-please-12345"));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        issuer: "DemoApi",
        audience: "DemoClient",
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(2), // short for demo
        signingCredentials: creds
    );
    return new JwtSecurityTokenHandler().WriteToken(token);
}

string CreateRefreshToken()
{
    var bytes = RandomNumberGenerator.GetBytes(64);
    return Convert.ToBase64String(bytes);
}

// Endpoints
app.MapPost("/api/auth/login", ([FromBody] LoginRequest req) =>
{
    // Demo users: admin/admin, user/user
    if ((req.Username == "admin" && req.Password == "admin") ||
        (req.Username == "user" && req.Password == "user"))
    {
        var access = CreateAccessToken(req.Username);
        var refresh = CreateRefreshToken();
        refreshStore[refresh] = req.Username;
        return Results.Ok(new TokenResponse(access, refresh, DateTime.UtcNow.AddMinutes(2)));
    }
    return Results.Unauthorized();
});

app.MapPost("/api/auth/refresh", ([FromBody] RefreshRequest req) =>
{
    if (req.RefreshToken is null) return Results.BadRequest();
    if (!refreshStore.TryGetValue(req.RefreshToken, out var username))
    {
        return Results.Unauthorized();
    }
    // rotate refresh token (best practice)
    refreshStore.Remove(req.RefreshToken);
    var newRefresh = CreateRefreshToken();
    refreshStore[newRefresh] = username;

    var access = CreateAccessToken(username);
    return Results.Ok(new TokenResponse(access, newRefresh, DateTime.UtcNow.AddMinutes(2)));
});

app.MapGet("/api/products", [Authorize] () =>
{
    var data = Enumerable.Range(1, 5).Select(i => new ProductDto(i, $"Product {i}"));
    return Results.Ok(data);
});

app.Run();
