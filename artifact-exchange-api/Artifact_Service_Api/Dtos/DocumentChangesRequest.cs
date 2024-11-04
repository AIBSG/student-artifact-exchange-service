namespace Artifact_Service_Api.Dtos
{
    public class DocumentChangesRequest
    {
        public Guid DocumentId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public IEnumerable<string>? TagsNames { get; set; }
    }
}
