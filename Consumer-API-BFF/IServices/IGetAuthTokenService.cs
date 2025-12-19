using Consumer_API_BFF.Models;

namespace Consumer_API_BFF.IServices
{
    public interface IGetAuthTokenService
    {
        public Task<string> GetAccessToken(AuthenticationModel authenticationModel,CancellationToken cancellationToken);
    }
}
