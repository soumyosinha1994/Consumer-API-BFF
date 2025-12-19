using Consumer_API_BFF.IServices;
using Hyland.MCA.Models;
using System.Text.Json;

namespace Consumer_API_BFF.Services
{
    public class GetJobIdForGetFieldsService : IGetJobIdForGetFieldsService
    {
        private readonly string _connection_ID;
        private readonly string _base_Url;
        private readonly IHttpClientFactory _httpClientFactory;
        public GetJobIdForGetFieldsService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _connection_ID = configuration["CONNECTION_ID"]!;
            _base_Url = configuration["BASE_URL"]!;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string> GetJobIdAsync(string contentId, string authToken, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            var fieldsUrl = $"{_base_Url}/connections/{_connection_ID}/contents/{contentId}/fields";

            var request = new HttpRequestMessage(HttpMethod.Get, fieldsUrl);
            request.Headers.Add("Authorization", $"Bearer {authToken}");
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
