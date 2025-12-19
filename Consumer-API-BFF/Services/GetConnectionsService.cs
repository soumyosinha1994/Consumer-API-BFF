using Consumer_API_BFF.IServices;

namespace Consumer_API_BFF.Services
{
    public class GetConnectionsService : IGetConnectionsService
    {
        private readonly string _base_Url;
        private readonly IHttpClientFactory _httpClientFactory;
        public GetConnectionsService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _base_Url = configuration["BASE_URL"]!;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string> PollConnection(string connectionId, string authToken, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            var pollResponseBody = string.Empty;
            var pollUrl = string.IsNullOrEmpty(connectionId)? $"{_base_Url}/connections": $"{_base_Url}/connections/{connectionId}";
            var pollRequest = new HttpRequestMessage(HttpMethod.Get, pollUrl);
            pollRequest.Headers.Add("Authorization", $"Bearer {authToken}");
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
