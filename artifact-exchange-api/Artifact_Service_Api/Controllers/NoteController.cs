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
    private readonly EditNoteRequest _request;
    

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
            .Where(n => n.Id.Equals(id))
            .Include(n => n.NoteAccess)
            .Include(n => n.NoteTags)
            .Include(n => n.Author)
            .Include(n => n.Files)
            .Select(n => n.Files)
            .FirstOrDefaultAsync();
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
        note.NoteAccess.Add(new NoteAccess {
            UserId = note.Author.Id,
            NoteId = note.Id,
            CanEdit = true
        });

        await _context.Notes.AddAsync(note);
        await _context.SaveChangesAsync();
        return Created();
    }

    [HttpGet]
    public async Task<IActionResult> GetNoteRequest(Guid id, Guid userId)
    {
        var note = await _context.Notes.FindAsync(id);
        if (note == null) return NotFound();

        var userAccess = await _context.NoteAccesses.Where(n => n.NoteId == id && n.UserId == userId).FirstOrDefaultAsync();
        if (!userAccess.CanEdit) return Forbid();
        
        _request.NoteId = note.Id;
        _request.Title = note.Title;
        _request.Description = note.Description;
        _request.Text = note.Text;
        _request.IsOpen = note.IsOpen;
        _request.TagsNames = await _context.NoteTags.Where(nt => nt.NoteId == id).Select(nt => nt.Tag.Name).ToListAsync();
        _request.FilesNames = note.Files.Select(f => f.CustomFileName);

        return Ok(_request);
    }

    [HttpPut]
    public async Task<IActionResult> EditNote(EditNoteRequest request, string serverFileName)
    {
        request = _request;
        var note = await _context.Notes.FindAsync(request.NoteId);
        
        note.Title = request.Title;
        note.Description = request.Description;
        note.Text = request.Text;
        note.IsOpen = request.IsOpen;
        var newTags = await _context.Tags.Where(t => request.TagsNames.Contains(t.Name)).ToListAsync();
        foreach (var tag in newTags)
        {
            if (!note.NoteTags.Any(nt => nt.TagId == tag.Id))
            {
                var noteTag = await _context.NoteTags.FindAsync(tag.Id);
                if (noteTag == null)
                {
                    note.NoteTags.Add(new NoteTag{
                        NoteId = note.Id,
                        TagId = tag.Id 
                    });
                    continue;
                }
                note.NoteTags.Remove(noteTag);
            }
        }
        foreach (var fileName in request.FilesNames)
        {
            if (!note.Files.Any(f => f.CustomFileName == fileName))
            {
                var file = await _context.Files.FindAsync(fileName, note.Id);
                if (file != null) note.Files.Remove(file);
            }
        }

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteNote(Guid id)
    {
        var note = await _context.Notes.FindAsync(id);
        if (note == null) return NotFound();

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
        return Ok();
    }
}