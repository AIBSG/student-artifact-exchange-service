using System.ComponentModel.DataAnnotations;

namespace Artifact_Service_Api.Models;

public class File : BaseEntity
{
    public string? CustomFileName { get; set; }
    public string? ServerFileName { get; set; }
    //public DocumentNote? DocumentNote { get; set; }
    //public Note? Note { get; set; }
}