using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Artifact_Service_Api.Models;

public class User : BaseEntity
{

    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public bool ActivatedEmail { get; set; }

    public int? RegistryCode { get; set; }

    public bool GAcoount { get; set; }
    [JsonIgnore]
    public virtual List<DocumentNoteAccess> DocumentNoteAccesses { get; set; }
    [JsonIgnore]
    public virtual List<NoteAccess> NoteAccess { get; set; }

}