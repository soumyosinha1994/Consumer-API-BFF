using System.Text.Json.Serialization;

namespace Consumer_API_BFF.Models
{
    public class CFGatewayEnums
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ProcessStatus
        {
            PENDING,
            SUBMITTED,
            FAILED,
            COMPLETED
        }
    }
}
