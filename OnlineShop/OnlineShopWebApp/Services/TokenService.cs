using System.Text;
using System.Text.Json;

namespace OnlineShopWebApp.Services
{
    public class TokenService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _cachedToken;
        private DateTime _tokenExpiry = DateTime.MinValue;

        public TokenService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetTokenAsync()
        {
            if (!string.IsNullOrEmpty(_cachedToken) && DateTime.UtcNow < _tokenExpiry)
            {
                return _cachedToken;
            }
                        
            var loginData = new
            {
                UserName = "admin",
                Password = "admin"
            };

            var json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/Authentication/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<JwtTokenResponse>(
                    responseJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                _cachedToken = tokenResponse?.Token;
                _tokenExpiry = DateTime.UtcNow.AddMinutes(5); 

                return _cachedToken;
            }

            throw new Exception($"Не смог получить токен: {response.StatusCode}");
        }
    }

    public class JwtTokenResponse
    {
        public string Token { get; set; }
    }
}