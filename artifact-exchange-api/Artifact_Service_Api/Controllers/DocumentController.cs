using Microsoft.AspNetCore.Mvc;
using Artifact_Service_Api.Service;
using Artifact_Service_Api.Dtos;
using Artifact_Service_Api.AppData;
using Microsoft.OpenApi.Validations;
using Microsoft.EntityFrameworkCore;
using Artifact_Service_Api.Models;
using System.Reflection.Metadata;

namespace Artifact_Service_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DocumentController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IFileStorageService _fileStorageService;
        private readonly AppDbContext _context;

        public DocumentController(IFileService fileService, IFileStorageService fileStorageService, AppDbContext context )
        {
            _fileService = fileService;
            _fileStorageService = fileStorageService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOpenDocuments() =>
         Ok(await _fileService.GetAllOpenDocuments());
        
            

        [HttpGet]
        public async Task<IActionResult> GetUserDocuments(Guid userId) =>
            Ok(_fileService.GetAllUserDocuments(userId));

        [HttpGet]
        public async Task<IActionResult> GetDocumentFile([FromQuery] Models.File file) =>
         File(_fileStorageService.GetFileBites(file).Result, 
             MimeTypes.GetMimeType(file.ServerFileName), 
             file.CustomFileName);

        [HttpPost]
        public async Task<IActionResult> SaveNewDocument (NewDocumentRequest request)
        {
            var serverFileName = _fileStorageService.SaveNewFileInStorage(request.File);
            return Ok(await _fileService.SaveNewDocument(request, serverFileName));

        }

        [HttpPatch]
        public async Task<IActionResult> SaveCangesDocument(DocumentChangesRequest request)
        {
            var document = await _context.DocumentNotes
                .Include(x => x.DocumentNoteTags)
                .Where(x => x.Id.Equals(request.DocumentId)).FirstOrDefaultAsync();
            document.Title = request.Title;
            document.Description = request.Description;
            if (document == null) return BadRequest();
            //var pastTags = await _context.DocumentNoteTags.Where(x => x.Id.Equals(request.DocumentId)).FirstOrDefaultAsync();
            document.DocumentNoteTags = new List<DocumentNoteTag>();
            var currentTags = await _context.Tags.Where(x => request.TagsNames.Contains(x.Name)).ToListAsync();

            foreach (var tag in currentTags)
            {
                document.DocumentNoteTags.Add(new DocumentNoteTag()
                {
                    DocumentNote = document,
                    DocumentNoteId = document.Id,
                    Id = Guid.NewGuid(),
                    Tag = tag,
                    TagId = tag.Id
                });
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDocument(Guid documnetId)
        {
            var document = await _context.DocumentNotes.Where(x => x.Id.Equals(documnetId)).FirstOrDefaultAsync();
            if (document == null) return BadRequest();
            _context.DocumentNotes.Remove(document);
            await _context.SaveChangesAsync();
            return Ok();   
        }


    }
}
