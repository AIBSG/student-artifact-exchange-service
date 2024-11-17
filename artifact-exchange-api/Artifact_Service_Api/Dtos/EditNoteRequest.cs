using Artifact_Service_Api.Models;

namespace Artifact_Service_Api.Dtos;

public class EditNoteRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Text { get; set; }
    public bool IsOpen { get; set; }
    public IEnumerable<string>? TagsNames { get; set; }
    public List<IFormFile>? Files { get; set; }
}