using Artifact_Service_Api.Models;

namespace Artifact_Service_Api.Dtos;

public class EditNoteRequest
{
    public Guid NoteId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Text { get; set; }
    public bool IsOpen { get; set; }
    public IEnumerable<string>? TagsNames { get; set; }
    public IEnumerable<string>? FilesNames { get; set; }
    public IEnumerable<IFormFile>? Files { get; set; }
    public IEnumerable<string>? Emails { get; set; }
    public IEnumerable<bool>? CanEdit { get; set; }
}