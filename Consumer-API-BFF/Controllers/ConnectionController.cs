using Consumer_API_BFF.IServices;
using Consumer_API_BFF.Models;
using Hyland.ContentFederation.CompressDecompress.Payload.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Consumer_API_BFF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConnectionController : Controller
    {
        private readonly IConnectionsService _getConnectionsService;
        public ConnectionController(IConnectionsService getConnectionsService)
        {
            _getConnectionsService = getConnectionsService;
        }

        [HttpGet("/get-connection")]
        public async Task<ActionResult> RetrieveConnections([FromHeader(Name = "Authorization")] string authorization, string connectionId = "")
        {
            if (string.IsNullOrEmpty(authorization))
            {
                return Unauthorized("Authorization header is missing");
            }

            var bearerToken = authorization.ToString();

            if (string.IsNullOrEmpty(bearerToken))
            {
                Console.WriteLine("Failed to get access token");
            }
            var pollResponseBody = await _getConnectionsService.PollConnection(connectionId, bearerToken, CancellationToken.None);

            if (!string.IsNullOrEmpty(connectionId))
            {
                var detailedResponse = JsonSerializer.Deserialize<ConnectionDetailsResponse>(pollResponseBody, JsonFormatUtility.DefaultJsonOptions);
                return Ok(detailedResponse);
            }
            else
            {
                var response = JsonSerializer.Deserialize<ConnectionListResponse>(pollResponseBody, JsonFormatUtility.DefaultJsonOptions);
                return Ok(response);
            }
        }

        [HttpPost("/add-connection")]
        public async Task<ActionResult> AddConnections([FromHeader(Name = "Authorization")] string authorization, [FromBody] CreateConnectionRequest createConnectionRequest)
        {
            if (string.IsNullOrEmpty(authorization))
            {
                return Unauthorized("Authorization header is missing");
            }

            var bearerToken = authorization.ToString();

            if (string.IsNullOrEmpty(bearerToken))
            {
                Console.WriteLine("Failed to get access token");
            }
            var pollResponseBody = await _getConnectionsService.CreateConnection(bearerToken, createConnectionRequest, CancellationToken.None);
            var response = JsonSerializer.Deserialize<CreateConnectionResponse>(pollResponseBody, JsonFormatUtility.DefaultJsonOptions);
            return Ok(response);
        }

        [HttpDelete("/delete-connection")]
        public async Task<ActionResult> DeleteConnections([FromHeader(Name = "Authorization")] string authorization, string connectionId)
        {
            if (string.IsNullOrEmpty(authorization))
            {
                return Unauthorized("Authorization header is missing");
            }

            var bearerToken = authorization.ToString();

            if (string.IsNullOrEmpty(bearerToken))
            {
                Console.WriteLine("Failed to get access token");
            }
            var pollResponseBody = await _getConnectionsService.DeleteConnection(connectionId, bearerToken, CancellationToken.None);
            var response = JsonSerializer.Deserialize<DeleteConnectionResponse>(pollResponseBody, JsonFormatUtility.DefaultJsonOptions);
            return Ok(response);
        }

    }
}
