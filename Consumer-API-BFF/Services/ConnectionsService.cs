using Consumer_API_BFF.IServices;
using Consumer_API_BFF.Models;
using Hyland.MCA.Models;
using System.Net.Http.Headers;

namespace Consumer_API_BFF.Services
{
    public class ConnectionsService : IConnectionsService
    {
        private readonly string _base_Url;
        private readonly IHttpClientFactory _httpClientFactory;
        public ConnectionsService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _base_Url = configuration["BASE_URL"]!;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> CreateConnection(string authToken, CreateConnectionRequest createConnectionRequest, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            var url = $"{_base_Url}/connections";
            var token = authToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? authToken : $"Bearer {authToken}";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
            var response = await client.PostAsJsonAsync(url, createConnectionRequest, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            return responseBody;
        }

        public async Task<string> DeleteConnection(string connectionId, string authToken, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            var url = $"{_base_Url}/connections/{connectionId}";
            var token = authToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? authToken : $"Bearer {authToken}";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
            var response = await client.DeleteAsync(url,cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            return responseBody;
        }

        public async Task<string> PollConnection(string connectionId, string authToken, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            var pollResponseBody = string.Empty;
            var pollUrl = string.IsNullOrEmpty(connectionId)? $"{_base_Url}/connections": $"{_base_Url}/connections/{connectionId}";
            var pollRequest = new HttpRequestMessage(HttpMethod.Get, pollUrl);
            var token = authToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? authToken : $"Bearer {authToken}";
            pollRequest.Headers.Add("Authorization", token);
            pollRequest.Headers.Add("Accept", "application/json");
            try
            {
                var pollResponse = await client.SendAsync(pollRequest);
                pollResponseBody = await pollResponse.Content.ReadAsStringAsync();
                return pollResponseBody;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error polling fields job: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
