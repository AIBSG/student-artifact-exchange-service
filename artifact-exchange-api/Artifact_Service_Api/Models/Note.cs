using System.ComponentModel.DataAnnotations;

namespace Artifact_Service_Api.Models;

public class Note
{
    [Key]
    public long NoteId { get; set; }

    public long AuthorId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Text { get; set; }

    public virtual User User { get; set; } = new User();

    public List<NoteAccess> NoteAccesses { get; set; } = [];
    public List<NoteTag> NoteTags { get; set; } = [];
    public List<NoteFile> NoteFiles { get; set; } = [];
}