using System.ComponentModel.DataAnnotations;

namespace Artifact_Service_Api.Models;

public class NoteFile
{
    [Key]
    public long NoteId { get; set; }

    [Key]
    public long FileId { get; set; }

    public virtual Note Note { get; set; } = new Note();
    public virtual File File { get; set; } = new File();
}