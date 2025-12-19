namespace Consumer_API_BFF.IServices
{
    public interface IGetJobIdForGetFieldsService
    {
     public Task<string> GetJobIdAsync(string contentId, string authToken, CancellationToken cancellationToken);
    }
}
