using System.ComponentModel.DataAnnotations;

namespace Artifact_Service_Api.Models
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
