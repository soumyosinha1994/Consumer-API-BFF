namespace Consumer_API_BFF.IServices
{
    public interface IPollContentTypesService
    {
        public Task<string> PollGetContentTypesService(string contentTypeId, string authToken, string operation, CancellationToken cancellationToken);
    }
}
