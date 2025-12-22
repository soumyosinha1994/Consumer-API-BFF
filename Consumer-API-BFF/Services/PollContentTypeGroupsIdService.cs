using Consumer_API_BFF.IServices;

namespace Consumer_API_BFF.Services
{
    public class PollContentTypeGroupsIdService : IPollContentTypeGroupsIdService
    {
        private readonly string _connection_ID;
        private readonly string _base_Url;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGetJobIdForContentTypeGroupIdService _getJobIdForContentTypeGroupsIdService;
        public PollContentTypeGroupsIdService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IGetJobIdForContentTypeGroupIdService getJobIdForContentTypeGroupsIdService)
        {
            _connection_ID = configuration["CONNECTION_ID"]!;
            _base_Url = configuration["BASE_URL"]!;
            _httpClientFactory = httpClientFactory;
            _getJobIdForContentTypeGroupsIdService = getJobIdForContentTypeGroupsIdService;
        }
        public async Task<string> PollGetContentTypeGroupsIdService(string contentTypeGroupId, string authToken, string operation, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            var pollResponseBody = string.Empty;
            var jobId = await _getJobIdForContentTypeGroupsIdService.GetJobIdAsync(contentTypeGroupId, authToken, operation, cancellationToken);
            if (!string.IsNullOrEmpty(jobId))
            {
                var pollUrl = $"{_base_Url}/connections/{_connection_ID}/content-type-groups/{contentTypeGroupId}/jobs/{jobId}";
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
