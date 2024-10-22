using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artifact_Service_Api.Models;

public class DocumentNoteAccess : BaseEntity
{
    [ForeignKey("User")]
    public Guid UserId { get; set; } 

    [ForeignKey("DocumentNote")]
    public Guid DocumentNoteId { get; set; } 
    public virtual User User { get; set; } 
    public virtual DocumentNote DocumentNote { get; set; }

}