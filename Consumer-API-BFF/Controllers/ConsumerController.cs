using Consumer_API_BFF.IServices;
using Consumer_API_BFF.Models;
using Hyland.ContentFederation.CompressDecompress.Payload.Utility;
using Hyland.ContentFederation.Shared.Common.Library.Models;
using Hyland.MCA.Enums;
using Hyland.MCA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Consumer_API_BFF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConsumerController : Controller
    {

        private readonly IPollFieldsService _pollFieldsService;
        private readonly IPollContentTypeGroupsService _pollContentTypeGroupsService;
        private readonly IPollContentTypeGroupsIdService _pollContentTypeGroupsIdService;
        private readonly IPollContentTypesService _pollContentTypesIdService;
        private readonly IPollDataObjectQueriesService _pollDataObjectQueriesService;
        private readonly IPollDataObjectQueriesByIdService _pollDataObjectQueriesByIdService;
        private readonly IPollExecuteObjectDataQueriesService _pollExecuteObjectDataQueriesService;
        private readonly IPollStandardSearchService _pollStandardSearchService;


        public ConsumerController(IHttpClientFactory httpClientFactory, IPollFieldsService pollFieldsService, IPollContentTypeGroupsService pollContentTypeGroupsService,
            IConnectionsService getConnectionsService, IPollContentTypeGroupsIdService pollContentTypeGroupsIdService, IPollContentTypesService pollContentTypesIdService,
            IPollDataObjectQueriesService pollDataObjectQueriesService, IPollDataObjectQueriesByIdService pollDataObjectQueriesByIdService, IPollExecuteObjectDataQueriesService pollExecuteObjectDataQueriesService, 
            IPollStandardSearchService pollStandardSearchService)
        {
            _pollFieldsService = pollFieldsService;
            _pollContentTypeGroupsService = pollContentTypeGroupsService;
            _pollContentTypeGroupsIdService = pollContentTypeGroupsIdService;
            _pollContentTypesIdService = pollContentTypesIdService;
            _pollDataObjectQueriesService = pollDataObjectQueriesService;
            _pollDataObjectQueriesByIdService = pollDataObjectQueriesByIdService;
            _pollExecuteObjectDataQueriesService = pollExecuteObjectDataQueriesService;
            _pollStandardSearchService = pollStandardSearchService;
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

        [HttpGet("/content-type-groups-Id/{contentTypeGroupId}")]
        public async Task<ActionResult> RetrieveContentTypeGroupsById(string contentTypeGroupId, [FromHeader(Name = "Authorization")] string authorization, [FromQuery, Required] Operation operation)
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
            var pollResponseBody = await _pollContentTypeGroupsIdService.PollGetContentTypeGroupsIdService(contentTypeGroupId, bearerToken, operation.ToString(), CancellationToken.None);
            var response = JsonSerializer.Deserialize<JobRetrievalResponse<ContentTypeGroup>>(pollResponseBody, JsonFormatUtility.DefaultJsonOptions);
            return Ok(response);
        }

        [HttpGet("/content-types/{contentTypeId}")]
        public async Task<ActionResult> RetrieveContentType(string contentTypeId, [FromHeader(Name = "Authorization")] string authorization, [FromQuery, Required] Operation operation)
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
            var pollResponseBody = await _pollContentTypesIdService.PollGetContentTypesService(contentTypeId, bearerToken, operation.ToString(), CancellationToken.None);
            var response = JsonSerializer.Deserialize<JobRetrievalResponse<ContentType>>(pollResponseBody, JsonFormatUtility.DefaultJsonOptions);
            return Ok(response);
        }

        [HttpGet("/data-objects-queries")]
        public async Task<ActionResult> RetrieveDataObjectQueries([FromHeader(Name = "Authorization")] string authorization, [FromQuery] int? offset = 0, [FromQuery] int? pageSize = 0)
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
            var pollResponseBody = await _pollDataObjectQueriesService.PollDataObjectQueries(bearerToken, offset, pageSize, CancellationToken.None);
            var response = JsonSerializer.Deserialize<JobRetrievalResponse<CollectionResult<DataObjectQueryTypeAbrv>>>(pollResponseBody, JsonFormatUtility.DefaultJsonOptions);
            return Ok(response);
        }

        [HttpGet("/data-objects-queries-by-id/{queryId}")]
        public async Task<ActionResult> RetrieveDataObjectQueriesById(string queryId, [FromHeader(Name = "Authorization")] string authorization)
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
            var pollResponseBody = await _pollDataObjectQueriesByIdService.PollPollDataObjectQueriesByIdAsync(queryId, bearerToken, CancellationToken.None);
            var response = JsonSerializer.Deserialize<JobRetrievalResponse<DataObjectQueryType>>(pollResponseBody, JsonFormatUtility.DefaultJsonOptions);
            return Ok(response);
        }

        [HttpPost("/execute-data-objects-queries/{queryId}")]
        public async Task<ActionResult> ExecuteDataObjectQueries(string queryId, [FromHeader(Name = "Authorization")] string authorization, [FromBody] DataObjectQuery dataObjectQuery, [FromQuery] int? offset = 0, [FromQuery] int? pageSize = 0)
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
            var pollResponseBody = await _pollExecuteObjectDataQueriesService.PollExecuteObjectDataQueries(queryId, bearerToken, dataObjectQuery, offset, pageSize, CancellationToken.None);
            var response = JsonSerializer.Deserialize<JobRetrievalResponse<CollectionResult<DataObject>>>(pollResponseBody, JsonFormatUtility.DefaultJsonOptions);
            return Ok(response);
        }

        [HttpPost("/standard-search")]
        public async Task<ActionResult> ExecuteDataObjectQueries([FromHeader(Name = "Authorization")] string authorization, [FromBody] ContentSearchRequest contentSearchRequest, [FromQuery] int offset, [FromQuery] int pageSize)
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
            var pollResponseBody = await _pollStandardSearchService.PollStandardSearch(bearerToken, contentSearchRequest, offset, pageSize, CancellationToken.None);
            var response = JsonSerializer.Deserialize<JobRetrievalResponse<CollectionResult<SearchContentResult>>>(pollResponseBody, JsonFormatUtility.DefaultJsonOptions);
            return Ok(response);
        }
    }
}