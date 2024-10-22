using System.ComponentModel.DataAnnotations;

namespace Artifact_Service_Api.Models;

public class Tag
{
    [Key]
    public long TagId { get; set; }

    public string? Name { get; set; }

    public List<NoteTag> NoteTags { get; set; } = [];
    public List<DocumentNoteTag> DocumentNoteTags { get; set; } = [];
}