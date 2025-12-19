namespace Consumer_API_BFF.IServices
{
    public interface IGetConnectionsService
    {
        public Task<string> PollConnection(string connectionId, string authToken, CancellationToken cancellationToken);
    }
}
