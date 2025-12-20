using Consumer_API_BFF.Models;

namespace Consumer_API_BFF.IServices
{
    public interface IConnectionsService
    {
        public Task<string> PollConnection(string connectionId, string authToken, CancellationToken cancellationToken);
        public Task<string> CreateConnection(string authToken, CreateConnectionRequest createConnectionRequest, CancellationToken cancellationToken);
        public Task<string> DeleteConnection(string connectionId, string authToken, CancellationToken cancellationToken);
    }
}
