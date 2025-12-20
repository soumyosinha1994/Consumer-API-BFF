namespace Consumer_API_BFF.Models
{
    public class CreateConnectionRequest
    {
        public string ConnectionName { get; set; }
        public string Description { get; set; }
        public Guid IntegrationId { get; set; }
        public PluginConfiguration PluginConfiguration { get; set; }
        public Guid EnvironmentId { get; set; }
        public string TransportType { get; set; }  // e.g. "DIRECT"
        public string ServiceUrl { get; set; } = "fcc-cf-sharepoint-plugin-host-api";
    }
    public record PluginConfiguration(string Type, string Id) { }
    public enum TransportType
    {
        DIRECT,
        INDIRECT
    }

}
