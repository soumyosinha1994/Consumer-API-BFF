using System.Text.Json.Serialization;

namespace Consumer_API_BFF.Models
{
    public class JobInitiationResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? JobId { get; set; }
    }
}
