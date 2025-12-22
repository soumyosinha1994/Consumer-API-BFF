namespace Consumer_API_BFF.IServices
{
    public interface IGetJobIdForDataObjectQueriesService
    {
        public Task<string> GetJobIdAsync(string authToken, int? offset, int? pageSize, CancellationToken cancellationToken);
    }
}
