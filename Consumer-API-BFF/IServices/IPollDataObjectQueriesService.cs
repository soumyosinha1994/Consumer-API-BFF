namespace Consumer_API_BFF.IServices
{
    public interface IPollDataObjectQueriesService
    {
        public Task<string> PollDataObjectQueries(string authToken, int? offset, int? pageSize, CancellationToken cancellationToken);
    }
}
