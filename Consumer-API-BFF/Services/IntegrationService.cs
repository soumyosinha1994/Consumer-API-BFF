using Consumer_API_BFF.IServices;
using Hyland.MCA.Enums;
using System.Web;

namespace Consumer_API_BFF.Services
{
    public class IntegrationService : IIntegrationService
    {
        private readonly string _base_Url;
        private readonly IHttpClientFactory _httpClientFactory;
        public IntegrationService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            //_base_Url = configuration["BASE_URL"]!;
            _base_Url = "https://localhost:44386";
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string> PollIntegration(string integrationId, string authToken, int? pageSize, string? cursor, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (pageSize.HasValue && pageSize!=0)
                query["pageSize"] = pageSize.Value.ToString();

            if(!string.IsNullOrEmpty(cursor))
               query["cursor"] = cursor;

            if(!string.IsNullOrEmpty(integrationId))
                query["integrationId"] = integrationId;

            var uriBuilder = new UriBuilder(_base_Url)
            {
                Path = $"system-integrations",
                Query = query.ToString()
            };
            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);
            var token = authToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? authToken : $"Bearer {authToken}";
            request.Headers.Add("Authorization", token);
            request.Headers.Add("Accept", "application/json");
            var jobResponse = await client.SendAsync(request, cancellationToken);
            var jobResponseBody = await jobResponse.Content.ReadAsStringAsync(cancellationToken);
            return jobResponseBody;
        }
    }
}
