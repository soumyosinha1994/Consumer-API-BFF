using Consumer_API_BFF.IServices;

namespace Consumer_API_BFF.Services
{
    public class PollContentTypesService : IPollContentTypesService
    {
        private readonly string _connection_ID;
        private readonly string _base_Url;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGetJobIdForContentTypeService _getJobIdForContentTypeService;
        public PollContentTypesService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IGetJobIdForContentTypeService getJobIdForContentTypeService)
        {
            _connection_ID = configuration["CONNECTION_ID"]!;
            _base_Url = configuration["BASE_URL"]!;
            _httpClientFactory = httpClientFactory;
            _getJobIdForContentTypeService = getJobIdForContentTypeService;
        }

        public async Task<string> PollGetContentTypesService(string contentTypeId, string authToken, string operation, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            var pollResponseBody = string.Empty;
            var jobId = await _getJobIdForContentTypeService.GetJobIdAsync(contentTypeId, authToken, operation, cancellationToken);
            if (!string.IsNullOrEmpty(jobId))
            {
                var pollUrl = $"{_base_Url}/connections/{_connection_ID}/content-types/{contentTypeId}/jobs/{jobId}";
                var pollRequest = new HttpRequestMessage(HttpMethod.Get, pollUrl);
                var token = authToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? authToken : $"Bearer {authToken}";
                pollRequest.Headers.Add("Authorization", token);
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
