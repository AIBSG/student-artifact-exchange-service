using Artifact_Service_Api.Dtos;
using Artifact_Service_Api.Models;
using Artifact_Service_Api.Service;
using Artifact_Service_Api.AppData;
using Microsoft.AspNetCore.Mvc;

namespace Artifact_Service_Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TagController(ITagService tagService, AppDbContext context) : ControllerBase
{
    private readonly ITagService _tagService = tagService;
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAllTags() => Ok(await _tagService.GetAllTags());

    [HttpPost]
    public async Task<IActionResult> CreateTag([FromForm]string tagName)
    {
        if (string.IsNullOrEmpty(tagName)) return BadRequest();
        else if (_context.Tags.Any(t => t.Name == tagName)) return BadRequest();

        return Ok(await _tagService.CreateTag(tagName));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTag(Guid tagId)
    {
        var tag = await _context.Tags.FindAsync(tagId);
        if (tag == null) return NotFound();
        
        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
        return Ok();
    }
}