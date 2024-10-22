using System.ComponentModel.DataAnnotations;

namespace Artifact_Service_Api.Models;

public class Tag :BaseEntity
{
    public string? Name { get; set; }
    public virtual List<NoteTag> NoteTags { get; set; }
    public virtual List<DocumentNoteTag> DocumentNoteTags { get; set; } 
}