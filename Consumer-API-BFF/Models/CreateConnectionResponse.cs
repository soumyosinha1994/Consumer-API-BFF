namespace Consumer_API_BFF.Models
{
    public class CreateConnectionResponse
    {
        public ConnectionData Data { get; set; }
    }
    public class ConnectionData
    {
        public Guid IntegrationId { get; set; }
        public string ConnectionName { get; set; }
        public string Description { get; set; }
        public Guid ConnectionId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TransportType { get; set; }
        public string ServiceUrl { get; set; }
        public string ConnectivityStatus { get; set; }
    }
}
