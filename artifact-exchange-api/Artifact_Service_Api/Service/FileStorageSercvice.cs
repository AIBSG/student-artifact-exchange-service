using Artifact_Service_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;
using System.Runtime;
using System;
using System.Web;
using System.Data;

namespace Artifact_Service_Api.Service
{
    public class FileStorageSercvice : IFileStorageService
    {

        public FileStorageSercvice() { }

        private readonly string _root = AppDomain.CurrentDomain.BaseDirectory;
        private readonly string _storage = "FilesStorage";

        public async Task<byte[]>?  GetFileBites(Models.File file)
        {
            
            var filePath = Path.Combine(_root, _storage, file.ServerFileName);
            if (!System.IO.File.Exists(filePath)) return null;
            return await System.IO.File.ReadAllBytesAsync(filePath);
        }

        public IEnumerable<Task<byte[]>>? GetFilesBitesByNote(IEnumerable<Models.File> files) => 
            files.Select(GetFileBites);

        /*public IActionResult Upload(UploadModel upload)
{
    var FileDic = "Files";

    string FilePath = Path.Combine(hostingEnv.WebRootPath, FileDic);

    if (!Directory.Exists(FilePath))
        Directory.CreateDirectory(FilePath);
    
    foreach (var file in upload.File)
    {              
        var filePath = Path.Combine(FilePath, file.FileName);

        using (FileStream fs = System.IO.File.Create(filePath))
        {
            file.CopyTo(fs);
            continue;
        }
    }

    return View("Index");*/
        public string SaveNewFileInStorage(IFormFile file)
        {
            var serverFileName = string.Format("{0}{1}"
            , Guid.NewGuid().ToString("N")
            , Path.GetExtension(file.FileName));
            var path = Path.Combine(_root, _storage, serverFileName);
            using (FileStream fs = System.IO.File.Create(path))
            {
                file.CopyTo(fs);
                return serverFileName;
            }
        }
    }
}
