using Microsoft.AspNetCore.Mvc;
using Artifact_Service_Api.Service;
using Artifact_Service_Api.Dtos;
using Artifact_Service_Api.AppData;
using Microsoft.OpenApi.Validations;
using Microsoft.EntityFrameworkCore;
using Artifact_Service_Api.Models;

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
        public async Task<IActionResult> GetDocumentFile(Models.File file) =>
         Ok(File(_fileStorageService.GetFileBites(file).Result, 
             MimeTypes.GetMimeType(file.ServerFileName), 
             file.CustomFileName));

        [HttpPost]
        public async Task<IActionResult> SaveNewDocument (SaveDocumentRequest request)
        {
            var serverFileName = _fileStorageService.SaveNewFileInStorage(request.File);
            return Ok(await _fileService.SaveNewDocument(request, serverFileName));

        }
        
    }
}
