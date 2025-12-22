namespace Consumer_API_BFF.IServices
{
    public interface IPollDataObjectQueriesByIdService
    {
        public Task<string> PollPollDataObjectQueriesByIdAsync(string queryId, string authToken, CancellationToken cancellationToken);
    }
}
