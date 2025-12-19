namespace Consumer_API_BFF.IServices
{
    public interface IPollFieldsService
    {
        public Task<string> PollFieldsAsync(string contentId, string authToken, CancellationToken cancellationToken);
    }
}
