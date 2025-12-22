using Hyland.MCA.Models;

namespace Consumer_API_BFF.IServices
{
    public interface IPollExecuteObjectDataQueriesService
    {
        public Task<string> PollExecuteObjectDataQueries(string queryId, string authToken, DataObjectQuery dataObjectQuery, int? offset, int? pageSize, CancellationToken cancellationToken);
    }
}
