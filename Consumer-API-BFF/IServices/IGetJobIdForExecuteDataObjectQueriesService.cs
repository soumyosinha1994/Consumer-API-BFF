using Hyland.MCA.Models;

namespace Consumer_API_BFF.IServices
{
    public interface IGetJobIdForExecuteDataObjectQueriesService
    {
        public Task<string> GetJobIdAsync(string queryId, string authToken, DataObjectQuery dataObjectQuery, int? offset, int? pageSize, CancellationToken cancellationToken);
    }
}
