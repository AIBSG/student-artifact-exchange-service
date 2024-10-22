using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artifact_Service_Api.Models;

public class DocumentNoteTag: BaseEntity
{
    [ForeignKey("DocumentNote")]
    public Guid DocumentNoteId { get; set; }

    [ForeignKey("Tag")]
    public Guid TagId { get; set; }

    public virtual DocumentNote DocumentNote { get; set; }
    public virtual Tag Tag { get; set; }
}