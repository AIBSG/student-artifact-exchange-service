using System.ComponentModel.DataAnnotations;

namespace Artifact_Service_Api.Models;

public class DocumentNoteTag
{
    [Key]
    public long DocumentNoteId { get; set; }

    [Key]
    public long TagId { get; set; }

    public virtual DocumentNote DocumentNote { get; set; } = new DocumentNote();
    public virtual Tag Tag { get; set; } = new Tag();
}