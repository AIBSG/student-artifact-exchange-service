using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artifact_Service_Api.Models;

public class NoteTag : BaseEntity
{
    [ForeignKey("Note")]
    public Guid NoteId { get; set; }

    [ForeignKey("Tag")]
    public Guid TagId { get; set; }
    public virtual Note Note { get; set; }

    public virtual Tag Tag { get; set; }
}