using Artifact_Service_Api.AppData;
using Artifact_Service_Api.Models;
using Artifact_Service_Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Artifact_Service_Api.Service;

public class TagService : ITagService
{
    private readonly AppDbContext _context;

    public TagService(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Tag>> GetAllTags() =>
        await _context.Tags.ToListAsync();

    public async Task<Tag> CreateTag(string tagName)
    {
        var tag = new Tag {
            Id = Guid.NewGuid(),
            Name = tagName
        };

        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();

        return tag;
    }
}