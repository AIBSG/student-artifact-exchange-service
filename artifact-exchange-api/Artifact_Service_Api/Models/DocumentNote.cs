using System.ComponentModel.DataAnnotations;

namespace Artifact_Service_Api.Models;

public class DocumentNote : BaseEntity
{ 
    public string? Title { get; set; }

    public string? Description { get; set; }

    public virtual File File { get; set; }

    public virtual List<DocumentNoteAccess> DocumentNoteAccesses { get; set; }
    public virtual  List<DocumentNoteTag> DocumentNoteTags { get; set; } 
}