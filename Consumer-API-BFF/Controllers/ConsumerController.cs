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

        public ConsumerController(IHttpClientFactory httpClientFactory, IPollFieldsService pollFieldsService, IPollContentTypeGroupsService pollContentTypeGroupsService)
        {
            _pollFieldsService = pollFieldsService;
            _pollContentTypeGroupsService = pollContentTypeGroupsService;
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

           var pollResponseBody =  await _pollFieldsService.PollFieldsAsync(contentId, bearerToken, cancellationToken);
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
    }
}