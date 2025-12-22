namespace Consumer_API_BFF.IServices
{
    public interface IGetJobIdForContentTypeGroupIdService
    {
        public Task<string> GetJobIdAsync(string contentTypeGroupId, string authToken, string operation, CancellationToken cancellationToken);
    }
}
