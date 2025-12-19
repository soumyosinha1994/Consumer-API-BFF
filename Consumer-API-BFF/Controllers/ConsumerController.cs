using Consumer_API_BFF.IServices;
using Consumer_API_BFF.Models;
using Hyland.ContentFederation.CompressDecompress.Payload.Utility;
using Hyland.ContentFederation.Shared.Common.Library.Models;
using Hyland.MCA.Enums;
using Hyland.MCA.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Consumer_API_BFF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ConsumerController : Controller
    {

        private readonly IPollFieldsService _pollFieldsService;
        private readonly IPollContentTypeGroupsService _pollContentTypeGroupsService;
        private readonly IGetConnectionsService _getConnectionsService;

        public ConsumerController(IHttpClientFactory httpClientFactory, IPollFieldsService pollFieldsService, IPollContentTypeGroupsService pollContentTypeGroupsService, IGetConnectionsService getConnectionsService)
        {
            _pollFieldsService = pollFieldsService;
            _pollContentTypeGroupsService = pollContentTypeGroupsService;
            _getConnectionsService = getConnectionsService;
        }

        [HttpGet("{contentId}/fields")]
        public async Task<ActionResult<JobRetrievalResponse<Fields>>> RetrieveFields(string contentId, [FromHeader(Name = "Authorization")] string authorization, CancellationToken cancellationToken)
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

            var pollResponseBody = await _pollFieldsService.PollFieldsAsync(contentId, bearerToken, cancellationToken);
            var response = JsonSerializer.Deserialize<JobRetrievalResponse<Fields>>(pollResponseBody, JsonFormatUtility.DefaultJsonOptions);
            return Accepted(response);
        }

        [HttpGet("/content-type-groups")]
        public async Task<ActionResult> RetrieveAllContentTypeGroups([FromHeader(Name = "Authorization")] string authorization, [FromQuery, Required] Operation operation, [FromQuery] int? offset = 0, [FromQuery] int? pageSize = 0)
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
            var pollResponseBody = await _pollContentTypeGroupsService.PollGetContentTypeGroupsService(bearerToken, operation.ToString(), offset, pageSize, CancellationToken.None);
            var response = JsonSerializer.Deserialize<JobRetrievalResponse<CollectionResult<ContentTypeGroupAbrv>>>(pollResponseBody, JsonFormatUtility.DefaultJsonOptions);
            return Accepted(response);
        }

        [HttpGet("/get-connection")]
        public async Task<ActionResult> RetrieveConnections([FromHeader(Name = "Authorization")] string authorization, string connectionId="")
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
    }
}