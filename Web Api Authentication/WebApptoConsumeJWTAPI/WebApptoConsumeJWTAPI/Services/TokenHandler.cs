using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace WebApptoConsumeJWTAPI.Services
{
    public class TokenHandler:DelegatingHandler
    {
        private readonly TokenService _tokens;
        private readonly IHttpClientFactory _factory;
        public readonly IConfiguration _configuration;

        public TokenHandler(TokenService tokens,IHttpClientFactory factory,IConfiguration configuration)
        {
            _tokens = tokens;
            _factory = factory;
            _configuration = configuration;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = _tokens.GetAccessToken();
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var refreshToken = _tokens.GetRefreshToken();
                if (string.IsNullOrEmpty(refreshToken)) return response;
                var client = _factory.CreateClient();
                var tokenResponse = await client.PostAsync(_configuration["ApiSettings:RefreshEndpoint"],
                    new StringContent(JsonSerializer.Serialize(new { token = accessToken, refreshToken = refreshToken }),
                    Encoding.UTF8, "application/json"));
                if (!tokenResponse.IsSuccessStatusCode)
                {
                    await _tokens.ClearAsync();
                    return response;
                }
                var json = await tokenResponse.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                if (tokenData is null || !tokenData.TryGetValue("token", out var newAccessToken) || !tokenData.TryGetValue("refreshToken", out var newRefreshToken))
                {
                    await _tokens.ClearAsync();
                    return response;
                }
                await _tokens.SetTokensAsync(newAccessToken, newRefreshToken);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);
                response.Dispose();
                response = await base.SendAsync(request, cancellationToken);
            }
            return response;
        }
    }
}
