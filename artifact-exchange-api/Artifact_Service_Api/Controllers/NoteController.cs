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
        Guid id = Guid.NewGuid();
        var newPath = $"../wwwroot/noteFiles/{id}";
        var dirInfo = new DirectoryInfo(newPath);
        if (!dirInfo.Exists) dirInfo.Create();

        var note = new Note();
        note.Id = id;
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
                Tag = tag,
                TagId = tag.Id
            });
        }
        note.Files = [];
        foreach (var file in request.Files)
        {
            var path = $"../wwwroot/noteFiles/{note.Id}/{file.FileName}";
            using (var fs = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

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
    public async Task<IActionResult> GetEditNote(Guid noteId, Guid userId)
    {
        var note = await _context.Notes.FindAsync(noteId);
        var user = await _context.NoteAccesses.Where(n => n.NoteId == noteId && n.UserId == userId).FirstOrDefaultAsync();
        var request = new EditNoteRequest();

        if (note == null) return NotFound();
        if (user.CanEdit) return BadRequest();

        request.Title = note.Title;
        request.Description = note.Description;
        request.Text = note.Text;
        request.IsOpen = note.IsOpen;
        request.TagsNames = await _context.NoteTags.Where(n => n.NoteId == noteId).Select(n => n.Tag.Name).ToListAsync();
        request.Emails = await _context.NoteAccesses.Where(n => n.NoteId == noteId).Select(n => n.User.Email).ToListAsync();
        request.CanEdit = await _context.NoteAccesses.Where(n => n.NoteId == noteId).Select(n => n.CanEdit).ToListAsync();
        request.Files = [];
        foreach (var file in note.Files)
        {
            var filePath = $"../wwwroot/noteFiles/{noteId}/{file.CustomFileName}";
            IFormFile formFile = new FormFile(new FileStream(filePath, FileMode.Open), 0, new FileInfo(filePath).Length, null, file.CustomFileName);
            request.Files.Add(formFile);
        }
        
        return Ok(request);
    }

    [HttpPut]
    public async Task<IActionResult> EditNote(Guid id, EditNoteRequest request, string serverFileName)
    {
        var note = await _context.Notes.FindAsync(id);
        if (note == null) return NotFound();

        note.Title = request.Title;
        note.Description = request.Description;
        note.Text = request.Text;
        note.IsOpen = request.IsOpen;
        var currentTags = await _context.Tags.Where(t => request.TagsNames.Contains(t.Name)).ToListAsync();
        var tags = await _context.NoteTags.Where(nt => nt.NoteId == id).Select(nt => nt.Tag).ToListAsync();
        var takenTags = tags.TakeWhile(t => !currentTags.Contains(t));
        if (takenTags != null)
        {
            foreach (var tag in takenTags)
            {
                if (tags.Contains(tag))
                {
                    var noteTag = await _context.NoteTags.FindAsync(tag.Id);
                    note.NoteTags.Remove(noteTag);
                }
                else
                {
                    note.NoteTags.Add(new NoteTag{
                        Note = note,
                        NoteId = note.Id,
                        Tag = tag,
                        TagId = tag.Id
                    });
                }
            }
        }
        var previousFilenames = note.Files.Select(f => f.CustomFileName).ToList();
        var currentFilenames = request.Files.Select(f => f.FileName).ToList();
        if (currentFilenames != null)
        {
            var filenames = previousFilenames.TakeWhile(f => !currentFilenames.Contains(f));
            if (filenames != null)
            {
                foreach (var filename in filenames)
                {
                    if (note.Files.Any(f => f.CustomFileName.Contains(filename)))
                    {
                        var file = await _context.Files.FirstOrDefaultAsync(f => f.CustomFileName.Contains(filename));
                        var path = $"../wwwroot/noteFiles/{id}/{file.CustomFileName}";
                        var fileInfo = new FileInfo(path);

                        if (fileInfo.Exists)
                        {
                            fileInfo.Delete();
                            note.Files.Remove(file);
                        }
                    }
                    else
                    {
                        note.Files.Add(new Models.File {
                            Id = Guid.NewGuid(),
                            CustomFileName = filename,
                            ServerFileName = serverFileName
                        });

                        var uploadFile = request.Files.FirstOrDefault(f => f.FileName == filename);
                        var path = $"../wwwroot/noteFiles/{id}/{uploadFile.FileName}";
                        using (var fs = new FileStream(path, FileMode.Create))
                        {
                            await uploadFile.CopyToAsync(fs);
                        }
                    }
                }
            }
        }
        var previousEmails = await _context.NoteAccesses.Where(a => a.NoteId == id).Select(a => a.User.Email).ToListAsync();
        var currentEmails = request.Emails;
        var previousAccesses = await _context.NoteAccesses.Where(a => a.NoteId == id).Select(a => a.CanEdit).ToListAsync();
        var currentAccesses = request.CanEdit;
        var selectedEmails = previousEmails.TakeWhile(e => !currentEmails.Contains(e));
        var selectedAccesses = previousAccesses.TakeWhile(a => !currentAccesses.Contains(a)).ToList();
        if (selectedEmails != null)
        {
            foreach (var email in selectedEmails)
            {
                if (previousEmails.Contains(email))
                {
                    var canEdit = selectedAccesses.First();
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Contains(email));
                    var access = await _context.NoteAccesses.FirstOrDefaultAsync(a => a.UserId == user.Id);

                    note.NoteAccess.Remove(access);
                    selectedAccesses.Remove(canEdit);
                }
                else
                {
                    var canEdit = selectedAccesses.First();
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Contains(email));

                    note.NoteAccess.Add(new NoteAccess {
                        Note = note,
                        NoteId = note.Id,
                        User = user,
                        UserId = user.Id,
                        CanEdit = canEdit
                    });
                }
            }
        }

        _context.Notes.Update(note);
        await _context.SaveChangesAsync();
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