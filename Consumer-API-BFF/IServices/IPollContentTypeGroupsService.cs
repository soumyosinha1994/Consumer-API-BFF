namespace Consumer_API_BFF.IServices
{
    public interface IPollContentTypeGroupsService
    {
        public Task<string> PollGetContentTypeGroupsService(string authToken, string operation, int? offset, int? pageSize, CancellationToken cancellationToken);
    }
}
