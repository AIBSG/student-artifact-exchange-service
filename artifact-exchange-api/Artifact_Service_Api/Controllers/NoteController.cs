using Microsoft.AspNetCore.Mvc;
using Artifact_Service_Api.Models;
using Artifact_Service_Api.AppData;
using Artifact_Service_Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Artifact_Service_Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class NoteController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<IEnumerable<Note>> GetOpenNotes() =>
        await _context.Notes
            .Where(n => n.IsOpen)
            .Include(n => n.NoteTags)
            .Include(n => n.NoteAccess)
            .Include(n => n.Author)
            .ToListAsync();

    [HttpGet]
    public async Task<IActionResult> GetNote(Guid id)
    {
        var note = await _context.Notes
            .Include(n => n.NoteTags)
            .Include(n => n.Author)
            .Include(n => n.Files)
            .Include(n => n.NoteAccess)
            .FirstOrDefaultAsync(n => n.Id == id);
        if (note == null) return NotFound();
        return Ok(note);
    }

    [HttpGet]
    public async Task<IEnumerable<Note>> GetAllNotesByUser(Guid userId) =>
        await _context.Notes
            .Where(n => n.Author.Id == userId && n.IsOpen)
            .Include(n => n.NoteAccess)
            .Include(n => n.NoteTags)
            .ToListAsync();

    [HttpGet]
    public async Task<IEnumerable<Note>> GetMyNotes(Guid userId) =>
        await _context.Notes
            .Where(n => n.Author.Id == userId)
            .Include(n => n.NoteAccess)
            .Include(n => n.NoteTags)
            .ToListAsync();

    [HttpPost]
    public async Task<IActionResult> CreateNote(SaveNoteRequest request, string serverFileName)
    {
        var note = new Note();
        note.Author = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.AuthorId);
        note.Title = request.Title;
        note.Description = request.Description;
        note.Text = request.Text;
        note.IsOpen = request.IsOpen;
        note.NoteTags = [];
        var currentTags = await _context.Tags.Where(t => request.TagsNames.Contains(t.Name)).ToListAsync();
        foreach (var tag in currentTags)
        {
            note.NoteTags.Add(new NoteTag
            {
                Note = note,
                NoteId = note.Id,
                Id = Guid.NewGuid(),
                Tag = tag,
                TagId = tag.Id
            });
        }
        note.Files = [];
        foreach (var file in request.Files)
        {
            note.Files.Add(new Models.File {
                Id = Guid.NewGuid(),
                CustomFileName = file.FileName,
                ServerFileName = serverFileName
            });
        } 
        await _context.Notes.AddAsync(note);
        await _context.SaveChangesAsync();
        return Ok(note);
    }
}