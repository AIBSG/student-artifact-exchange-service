using System.ComponentModel.DataAnnotations;

namespace Artifact_Service_Api.Models;

public class NoteTag
{
    [Key]
    public long NoteId { get; set; }

    [Key]
    public long TagId { get; set; }

    public Note Note { get; set; } = new Note();

    public Tag Tag { get; set; } = new Tag();
}