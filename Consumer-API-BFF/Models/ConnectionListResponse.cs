namespace Consumer_API_BFF.Models
{
    public class ConnectionListResponse
    {
        public List<ConnectionItem> Data { get; init; } = [];
    }
    public class ConnectionItem
    {
        public Guid IntegrationId { get; init; }

        public string ConnectionName { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public Guid ConnectionId { get; init; }

        public string Status { get; init; } = string.Empty;

        public DateTimeOffset CreatedAt { get; init; }

        public string TransportType { get; init; } = string.Empty;

        public string ServiceUrl { get; init; } = string.Empty;

        public string ConnectivityStatus { get; init; } = string.Empty;
    }
}
