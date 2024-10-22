using System.ComponentModel.DataAnnotations;

namespace Artifact_Service_Api.Models;

public class NoteAccess
{
    [Key]
    public long NoteId { get; set; }

    [Key]
    public long UserID { get; set; }

    public bool CanEdit { get; set; }

    public virtual User User { get; set; } = new User();

    public virtual Note Note { get; set; } = new Note();
}