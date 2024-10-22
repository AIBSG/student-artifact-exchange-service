using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artifact_Service_Api.Models;

public class NoteAccess : BaseEntity
{
    [ForeignKey("User")]
    public Guid UserId { get; set; } 

    [ForeignKey("Note")]
    public Guid NoteId { get; set; }
    public bool CanEdit { get; set; }

    public virtual User User { get; set; } = new User();

    public virtual Note Note { get; set; } = new Note();
}