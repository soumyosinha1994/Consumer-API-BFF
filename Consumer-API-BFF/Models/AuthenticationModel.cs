namespace Consumer_API_BFF.Models
{
    public class AuthenticationModel
    {
        public required string Url { get; set; }
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
    }
}
