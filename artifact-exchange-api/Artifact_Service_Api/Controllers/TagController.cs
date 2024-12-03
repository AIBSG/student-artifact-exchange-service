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
    public async Task<IActionResult> CreateTag([FromForm]string tagName) =>
        Ok(await _tagService.CreateTag(tagName));
}