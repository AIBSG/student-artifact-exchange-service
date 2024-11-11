using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artifact_Service_Api.Models;

public class NoteFile : BaseEntity
{
    [ForeignKey("Note")]
    public Guid NoteId { get; set; }
    
    [ForeignKey("File")]
    public Guid FileId { get; set; }
    
    public virtual Note Note { get; set; } = new Note();
    public virtual File File { get; set; } = new File();
}