using Consumer_API_BFF.IServices;
using Consumer_API_BFF.Models;
using Hyland.MCA.Models;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Web;

namespace Consumer_API_BFF.Services
{
    public class GetJobIdForExecuteDataObjectQueriesService : IGetJobIdForExecuteDataObjectQueriesService
    {
        private readonly string _connection_ID;
        private readonly string _base_Url;
        private readonly IHttpClientFactory _httpClientFactory;
        
        public GetJobIdForExecuteDataObjectQueriesService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _connection_ID = configuration["CONNECTION_ID"]!;
            _base_Url = configuration["BASE_URL"]!;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string> GetJobIdAsync(string queryId, string authToken, DataObjectQuery dataObjectQuery, int? offset, int? pageSize, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();

            var query = HttpUtility.ParseQueryString(string.Empty);
            if (offset.HasValue)
                query["offset"] = offset.Value.ToString();

            if (pageSize.HasValue)
                query["pageSize"] = pageSize.Value.ToString();
            var uriBuilder = new UriBuilder(_base_Url)
            {
                Path = $"connections/{_connection_ID}/data-objects/queries/{queryId}",
                Query = query.ToString()
            };
            var requestUri = uriBuilder.Uri;
            var token = authToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? authToken : $"Bearer {authToken}";
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(token);
            var response = await client.PostAsJsonAsync(requestUri, dataObjectQuery, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            string jobId = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var jsonDoc = JsonDocument.Parse(responseBody);
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
