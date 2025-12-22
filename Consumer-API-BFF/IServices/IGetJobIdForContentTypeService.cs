namespace Consumer_API_BFF.IServices
{
    public interface IGetJobIdForContentTypeService
    {
        public Task<string> GetJobIdAsync(string contentTypeId, string authToken, string operation, CancellationToken cancellationToken);
    }
}
