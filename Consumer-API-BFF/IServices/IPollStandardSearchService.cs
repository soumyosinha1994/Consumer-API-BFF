using Consumer_API_BFF.Models;
using Hyland.MCA.Models;

namespace Consumer_API_BFF.IServices
{
    public interface IPollStandardSearchService
    {
        public Task<string> PollStandardSearch(string authToken, ContentSearchRequest contentSearchRequest, int? offset, int? pageSize, CancellationToken cancellationToken);
    }
}
