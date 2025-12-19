using System.ComponentModel.DataAnnotations;

namespace Consumer_API_BFF.Models
{
    public class CFAuthenticationConfig
    {
        [Required]
        public required string Authority { get; set; }

        public bool RequireHttpsMetadata { get; set; } = true;

        public required string Audience { get; set; }
        public required string ValidTokenTypes { get; set; }
    }
}
