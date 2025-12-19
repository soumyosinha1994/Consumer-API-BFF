using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Consumer_API_BFF.Models
{
    public class RequestHeader
    {
        [FromHeader(Name = "Authorization")]
        [Required(ErrorMessage = "Authorization token is required.")]
        public string? AuthToken { get; set; }
    }
}
