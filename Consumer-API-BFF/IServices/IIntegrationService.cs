using Microsoft.AspNetCore.Mvc;

namespace Consumer_API_BFF.IServices
{
    public interface IIntegrationService
    {
        public Task<string> PollIntegration(string integrationId, string authToken, int? pageSize, string? cursor, CancellationToken cancellationToken);
    }
}
