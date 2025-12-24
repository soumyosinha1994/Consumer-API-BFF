namespace Consumer_API_BFF.Models
{
    public class SystemIntegrationResponse
    {
        public required List<SystemIntegration> SystemIntegrations { get; set; } = new List<SystemIntegration>();
        public PageInfo? PageInfo { get; set; }
    }
    public class SystemIntegration
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
    }
    public class PageInfo
    {
        public required string EndCursor { get; set; }
        public required bool HasNextPage { get; set; }
    }
}
