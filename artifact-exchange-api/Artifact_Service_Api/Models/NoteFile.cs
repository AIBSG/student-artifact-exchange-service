using System.ComponentModel.DataAnnotations;

namespace Artifact_Service_Api.Models;

public class NoteFile
{
    public virtual Note Note { get; set; } = new Note();
    public virtual File File { get; set; } = new File();
}