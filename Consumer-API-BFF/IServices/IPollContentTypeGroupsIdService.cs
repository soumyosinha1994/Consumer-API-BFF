namespace Consumer_API_BFF.IServices
{
    public interface IPollContentTypeGroupsIdService
    {
        public Task<string> PollGetContentTypeGroupsIdService(string contentTypeGroupId, string authToken, string operation, CancellationToken cancellationToken);
    }
}
