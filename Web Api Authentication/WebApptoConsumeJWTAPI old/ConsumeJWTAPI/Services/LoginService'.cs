using ConsumeJWTAPI.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
//using System.Text.Json;

namespace ConsumeJWTAPI.Services
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        private TokenResponse _tokens;
        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7151/"); // Set your API base address here
        }

        public async Task<bool> LoginAsync(string username, string password,string role)
        {
            var loginData = new { username = username, password = password, position = role };
            var content = new StringContent(JsonConvert.SerializeObject(loginData), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/auth/login", content);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                _tokens = JsonConvert.DeserializeObject<TokenResponse>(json); //  <TokenResponse>(json);
                return true; // Return the JWT token
            }
            else
            {
                throw new Exception("Login failed: " + response.ReasonPhrase);
            }
        }


        private async Task<bool> RefreshTokenAsync()
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(new { refreshToken = _tokens.RefreshToken }),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("api/auth/refresh", content);
            if (!response.IsSuccessStatusCode) return false;

            var json = await response.Content.ReadAsStringAsync();
            _tokens = JsonConvert.DeserializeObject<TokenResponse>(json);

            return true;
        }
        // end of class

    }
}
