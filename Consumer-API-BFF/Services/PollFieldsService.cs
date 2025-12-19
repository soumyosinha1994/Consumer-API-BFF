using Consumer_API_BFF.IServices;

namespace Consumer_API_BFF.Services
{
    public class PollFieldsService : IPollFieldsService
    {
        private readonly string _connection_ID;
        private readonly string _base_Url;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGetJobIdForGetFieldsService _getJobIdForGetFieldsService;
        public PollFieldsService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IGetJobIdForGetFieldsService getJobIdForGetFieldsService)
        {
            _connection_ID = configuration["CONNECTION_ID"]!;
            _base_Url = configuration["BASE_URL"]!;
            _httpClientFactory = httpClientFactory;
            _getJobIdForGetFieldsService = getJobIdForGetFieldsService;
        }
        public async Task<string> PollFieldsAsync(string contentId, string authToken, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            var pollResponseBody = string.Empty;
            // Poll job status if jobId exists
            var jobId = await  _getJobIdForGetFieldsService.GetJobIdAsync(contentId, authToken, cancellationToken);
            if (!string.IsNullOrEmpty(jobId))
            {
                var pollUrl = $"{_base_Url}/connections/{_connection_ID}/contents/{contentId}/fields/jobs/{jobId}";

                var pollRequest = new HttpRequestMessage(HttpMethod.Get, pollUrl);
                pollRequest.Headers.Add("Authorization", $"Bearer {authToken}");
                pollRequest.Headers.Add("Accept", "application/json");
                await Task.Delay(500, cancellationToken);
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
            return pollResponseBody;
        }
    }
}
