namespace Consumer_API_BFF.Models
{
    public class DeleteConnectionResponse
    {
        public DeleteConnection Data { get; init; } = default!;
    }
    public class DeleteConnection
    {
        public Guid ConnectionId { get; set; }
        public Guid IntegrationId { get; set; }
        public Guid EnvironmentId { get; set; }

        public string ConnectionName { get; set; }
        public string Description { get; set; }

        public string PluginId { get; set; }
        public string PluginType { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Status { get; set; }
        public string TransportType { get; set; }   // keep string for extensibility
        public string ServiceUrl { get; set; }
    }
}
