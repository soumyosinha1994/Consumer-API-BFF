using Consumer_API_BFF.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using static Consumer_API_BFF.Models.CFGatewayEnums;

namespace Consumer_API_BFF.Models
{
    public class JobRetrievalResponse<T> : JobInitiationResponse
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ProcessStatus? ProcessStatus { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Result { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ProblemDetails? Error { get; set; }
    }
}
