using Artifact_Service_Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Artifact_Service_Api.Service;

public interface ITagService
{
    public Task<IEnumerable<Tag>> GetAllTags();
    public Task<Tag> CreateTag(string tagName);
}