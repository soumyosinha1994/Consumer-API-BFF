using Consumer_API_BFF.IServices;
using Hyland.MCA.Enums;

namespace Consumer_API_BFF.Services
{
    public class PollDataObjectQueriesService : IPollDataObjectQueriesService
    {
        private readonly string _connection_ID;
        private readonly string _base_Url;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGetJobIdForDataObjectQueriesService _getJobIdForDataObjectQueriesService;
        public PollDataObjectQueriesService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IGetJobIdForDataObjectQueriesService getJobIdForDataObjectQueriesService)
        {
            _connection_ID = configuration["CONNECTION_ID"]!;
            _base_Url = configuration["BASE_URL"]!;
            _httpClientFactory = httpClientFactory;
            _getJobIdForDataObjectQueriesService = getJobIdForDataObjectQueriesService;
        }
        public async Task<string> PollDataObjectQueries(string authToken, int? offset, int? pageSize, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            var pollResponseBody = string.Empty;
            var jobId = await _getJobIdForDataObjectQueriesService.GetJobIdAsync(authToken, offset, pageSize, cancellationToken);
            if (!string.IsNullOrEmpty(jobId))
            {
                var pollUrl = $"{_base_Url}/connections/{_connection_ID}/data-objects/queries/jobs/{jobId}";
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
