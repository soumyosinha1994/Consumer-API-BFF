namespace Consumer_API_BFF.IServices
{
    public interface IGetJobIdForDataObjectQueriesByIdService
    {
        public Task<string> GetJobIdAsync(string queryId, string authToken, CancellationToken cancellationToken);
    }
}
