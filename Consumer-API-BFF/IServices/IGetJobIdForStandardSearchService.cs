using Consumer_API_BFF.Models;

namespace Consumer_API_BFF.IServices
{
    public interface IGetJobIdForStandardSearchService
    {
        public Task<string> GetJobIdAsync(string authToken, ContentSearchRequest contentSearchRequest, int? offset, int? pageSize, CancellationToken cancellationToken);
    }
}
