using Consumer_API_BFF.IServices;
using Consumer_API_BFF.Models;
using System.Text.Json;

namespace Consumer_API_BFF.Services
{
    public class GetAuthTokenService : IGetAuthTokenService
    {
        private string _auth_Token_Url;
        private string _client_Id;
        private string _client_Secret;
        private readonly IHttpClientFactory _httpClientFactory;
        public GetAuthTokenService(IConfiguration configuration, IHttpClientFactory httpClientFactory) 
        {
            _auth_Token_Url = configuration["Auth_TOKEN_URL"]!;
            _client_Id = configuration["CLIENT_ID"]!;
            _client_Secret = configuration["CLIENT_SECRET"]!;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string> GetAccessToken(AuthenticationModel authenticationModel, CancellationToken cancellationToken)
        {
            try
            {
                _auth_Token_Url= string.IsNullOrEmpty(authenticationModel.Url)? _auth_Token_Url: authenticationModel.Url;
                _client_Id= string.IsNullOrEmpty(authenticationModel.ClientId)? _client_Id: authenticationModel.ClientId;
                _client_Secret = string.IsNullOrEmpty(authenticationModel.ClientSecret) ? _client_Secret : authenticationModel.ClientSecret;
                var client = _httpClientFactory.CreateClient();
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _client_Id),
                new KeyValuePair<string, string>("client_secret", _client_Secret)
            });

                var response = await client.PostAsync(_auth_Token_Url, content, cancellationToken);
                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

                Console.WriteLine($"Token Response: {responseBody}");

                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }

                var jsonDoc = JsonDocument.Parse(responseBody);
                if (jsonDoc.RootElement.TryGetProperty("access_token", out var tokenElement))
                {
                    return tokenElement.GetString()!;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting access token: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
