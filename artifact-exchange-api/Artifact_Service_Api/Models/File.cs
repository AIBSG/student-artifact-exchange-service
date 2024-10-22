using System.ComponentModel.DataAnnotations;

namespace Artifact_Service_Api.Models;

public class File
{
    [Key]
    public long FileId { get; set; }

    public string? CustomFileName { get; set; }

    public string? ServerFileName { get; set; }

    public string? FilePath { get; set; }

    public List<DocumentNote>? DocumentNotes { get; set; } = [];
    public List<NoteFile> NoteFiles { get; set; } = [];
}