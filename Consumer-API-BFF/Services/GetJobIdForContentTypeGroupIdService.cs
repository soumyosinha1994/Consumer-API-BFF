using Consumer_API_BFF.IServices;
using System.Text.Json;
using System.Web;

namespace Consumer_API_BFF.Services
{
    public class GetJobIdForContentTypeGroupIdService : IGetJobIdForContentTypeGroupIdService
    {
        private readonly string _connection_ID;
        private readonly string _base_Url;
        private readonly IHttpClientFactory _httpClientFactory;
        public GetJobIdForContentTypeGroupIdService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _connection_ID = configuration["CONNECTION_ID"]!;
            _base_Url = configuration["BASE_URL"]!;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string> GetJobIdAsync(string contentTypeGroupId, string authToken, string operation, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["operation"] = operation;
            var uriBuilder = new UriBuilder(_base_Url)
            {
                Path = $"connections/{_connection_ID}/content-type-groups/{contentTypeGroupId}",
                Query = query.ToString()
            };
            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);
            var token = authToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? authToken : $"Bearer {authToken}";
            request.Headers.Add("Authorization", token);
            request.Headers.Add("Accept", "application/json");

            var jobResponse = await client.SendAsync(request, cancellationToken);
            var jobResponseBody = await jobResponse.Content.ReadAsStringAsync(cancellationToken);

            string jobId = string.Empty;
            if (jobResponse.IsSuccessStatusCode)
            {
                try
                {
                    var jsonDoc = JsonDocument.Parse(jobResponseBody);
                    if (jsonDoc.RootElement.TryGetProperty("jobId", out var jobIdElement))
                    {
                        jobId = jobIdElement.GetString()!;
                    }
                    return jobId!;
                }
                catch
                {
                    return string.Empty;
                }
            }
            return jobId;
        }
    }
}
