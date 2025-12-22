using Hyland.MCA.Models;

namespace Consumer_API_BFF.Models
{
    public class ContentSearchRequest
    {
        public string? ContentTypeGroupId { get; set; }
        public string? ContentTypeId { get; set; }
        public List<SimpleFieldAbrv>? SimpleFields { get; set; }
        public List<ComplexFieldAbrv>? ComplexFields { get; set; }
        public DateTime? MinCreationDate { get; set; } //Date format is ISO 8601
        public DateTime? MaxCreationDate { get; set; } //Date format is ISO 8601
    }
}
