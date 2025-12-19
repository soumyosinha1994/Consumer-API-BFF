namespace Consumer_API_BFF.IServices
{
    public interface IGetJobIdForContentTypeGroupsService
    {
        public Task<string> GetJobIdAsync(string authToken, string operation, int? offset, int? pageSize, CancellationToken cancellationToken);
    }
}
