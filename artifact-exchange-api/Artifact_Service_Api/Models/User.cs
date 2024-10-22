using System.ComponentModel.DataAnnotations;

namespace Artifact_Service_Api.Models;

public class User
{
    [Key]
    public long Id { get; set; }

    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public bool ActivatedEmail { get; set; }

    public int RegistryCode { get; set; }

    public bool GAcoount { get; set; }

    public List<Note> Notes { get; set; } = [];
    public List<NoteAccess> NoteAccesses { get; set; } = [];
    public List<DocumentNoteAccess> DocumentNoteAccesses { get; set; } = [];
}