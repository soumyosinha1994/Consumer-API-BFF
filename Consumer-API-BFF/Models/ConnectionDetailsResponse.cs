namespace Consumer_API_BFF.Models
{
    public class ConnectionDetailsResponse
    {
        public ConnectionDetails Data { get; init; } = default!;
    }
    public class ConnectionDetails
    {
        public string ConnectivityStatus { get; init; } = string.Empty;

        public Guid ConnectionId { get; init; }

        public Guid IntegrationId { get; init; }

        public Guid EnvironmentId { get; init; }

        public string ConnectionName { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public Guid PluginId { get; init; }

        public string PluginType { get; init; } = string.Empty;

        public string CreatedBy { get; init; } = string.Empty;

        public DateTimeOffset CreatedAt { get; init; }

        public string Status { get; init; } = string.Empty;

        public string TransportType { get; init; } = string.Empty;

        public string ServiceUrl { get; init; } = string.Empty;
    }

}
