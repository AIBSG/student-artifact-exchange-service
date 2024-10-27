using System.ComponentModel.DataAnnotations;

namespace Artifact_Service_Api.Models;

public class Note : BaseEntity
{
    public virtual User Author { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Text { get; set; }
    public bool IsOpen { get; set; }
    public virtual List<NoteTag> NoteTags { get; set; }
    public virtual List<NoteAccess> NoteAccess { get; set; }
    public virtual List<File> Files { get; set; }

}