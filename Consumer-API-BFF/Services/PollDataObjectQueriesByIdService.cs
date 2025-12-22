using Consumer_API_BFF.IServices;
using Hyland.MCA.Models;

namespace Consumer_API_BFF.Services
{
    public class PollDataObjectQueriesByIdService : IPollDataObjectQueriesByIdService
    {
        private readonly string _connection_ID;
        private readonly string _base_Url;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGetJobIdForDataObjectQueriesByIdService _getJobIdForDataObjectQueriesByIdService;
        public PollDataObjectQueriesByIdService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IGetJobIdForDataObjectQueriesByIdService getJobIdForDataObjectQueriesByIdService)
        {
            _connection_ID = configuration["CONNECTION_ID"]!;
            _base_Url = configuration["BASE_URL"]!;
            _httpClientFactory = httpClientFactory;
            _getJobIdForDataObjectQueriesByIdService = getJobIdForDataObjectQueriesByIdService;
        }

        public async Task<string> PollPollDataObjectQueriesByIdAsync(string queryId, string authToken, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            var pollResponseBody = string.Empty;
            // Poll job status if jobId exists
            var jobId = await _getJobIdForDataObjectQueriesByIdService.GetJobIdAsync(queryId, authToken, cancellationToken);
            if (!string.IsNullOrEmpty(jobId))
            {
                var pollUrl = $"{_base_Url}/connections/{_connection_ID}/data-objects/queries/{queryId}/jobs/{jobId}";

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
