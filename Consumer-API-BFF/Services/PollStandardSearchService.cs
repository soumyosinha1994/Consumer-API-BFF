using Consumer_API_BFF.IServices;
using Consumer_API_BFF.Models;
using System.Net.Http.Headers;

namespace Consumer_API_BFF.Services
{
    public class PollStandardSearchService : IPollStandardSearchService
    {
        private readonly string _connection_ID;
        private readonly string _base_Url;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGetJobIdForStandardSearchService _getJobIdForStandardSearchService;
        public PollStandardSearchService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IGetJobIdForStandardSearchService getJobIdForStandardSearchService)
        {
            _connection_ID = configuration["CONNECTION_ID"]!;
            _base_Url = configuration["BASE_URL"]!;
            _httpClientFactory = httpClientFactory;
            _getJobIdForStandardSearchService = getJobIdForStandardSearchService;
        }

        public async Task<string> PollStandardSearch(string authToken, ContentSearchRequest contentSearchRequest, int? offset, int? pageSize, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            var pollResponseBody = string.Empty;
            var jobId = await _getJobIdForStandardSearchService.GetJobIdAsync(authToken, contentSearchRequest, offset, pageSize, cancellationToken);
            if (!string.IsNullOrEmpty(jobId))
            {
                var url = $"{_base_Url}/connections/{_connection_ID}/contents/standard-search/jobs/{jobId}";
                var token = authToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? authToken : $"Bearer {authToken}";
                client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(token);
                await Task.Delay(500, cancellationToken);
                try
                {
                    var response = await client.PostAsync(url, null, cancellationToken);
                    pollResponseBody = await response.Content.ReadAsStringAsync(cancellationToken);
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
