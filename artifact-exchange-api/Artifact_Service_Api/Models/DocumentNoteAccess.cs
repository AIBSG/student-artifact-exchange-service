using System.ComponentModel.DataAnnotations;

namespace Artifact_Service_Api.Models;

public class DocumentNoteAccess
{
    [Key]
    public long DocumentNoteId { get; set; }

    [Key]
    public long UserId { get; set; }

    public virtual User User { get; set; } = new User();
    public virtual DocumentNote DocumentNote { get; set; } = new DocumentNote();
}