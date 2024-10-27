using Artifact_Service_Api.Models;

namespace Artifact_Service_Api.Dtos
{
    public class SaveDocumentRequest
    {
        public Guid AuthorId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsOpen { get; set; }
        public IEnumerable<string> TagsNames { get; set; }
        public IFormFile File {  get; set; }
    }
}
