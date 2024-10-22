using System.ComponentModel.DataAnnotations;

namespace Artifact_Service_Api.Models;

public class DocumentNote
{
    [Key]
    public long DocumentNoteId { get; set; }

    public long FileId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public virtual File File { get; set; } = new File();

    public List<DocumentNoteAccess> DocumentNoteAccesses { get; set; } = [];
    public List<DocumentNoteTag> DocumentNoteTags { get; set; } = [];
}