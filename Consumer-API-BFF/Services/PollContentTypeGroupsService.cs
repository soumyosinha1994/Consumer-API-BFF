using Consumer_API_BFF.IServices;
using Hyland.MCA.Models;
using System.Web;

namespace Consumer_API_BFF.Services
{
    public class PollContentTypeGroupsService : IPollContentTypeGroupsService
    {
        private readonly string _connection_ID;
        private readonly string _base_Url;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGetJobIdForContentTypeGroupsService _getJobIdForContentTypeGroupsService;
        public PollContentTypeGroupsService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IGetJobIdForContentTypeGroupsService getJobIdForContentTypeGroupsService)
        {
            _connection_ID = configuration["CONNECTION_ID"]!;
            _base_Url = configuration["BASE_URL"]!;
            _httpClientFactory = httpClientFactory;
            _getJobIdForContentTypeGroupsService = getJobIdForContentTypeGroupsService;
        }

        public async Task<string> PollGetContentTypeGroupsService(string authToken, string operation, int? offset, int? pageSize, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            var pollResponseBody = string.Empty;
            var jobId = await _getJobIdForContentTypeGroupsService.GetJobIdAsync(authToken, operation, offset, pageSize, cancellationToken);
            if (!string.IsNullOrEmpty(jobId))
            {
                var pollUrl = $"{_base_Url}/connections/{_connection_ID}/content-type-groups/jobs/{jobId}";
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

