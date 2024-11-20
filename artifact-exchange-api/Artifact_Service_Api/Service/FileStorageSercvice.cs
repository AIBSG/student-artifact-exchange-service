using Artifact_Service_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;
using System.Runtime;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Artifact_Service_Api.Service
{
    public class FileStorageSercvice : IFileStorageService
    {

        private Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnv;
        //костыль надо исправить 
        private readonly string _storagePath = "C:\\Users\\gglol\\Desktop\\student-artifact-exchange-service\\artifact-exchange-api\\Artifact_Service_Api\\FileStorage\\"; 
       

        public FileStorageSercvice(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }

        public async Task<byte[]>?  GetFileBites(string serverFileName)
        {
            
            var filePath = Path.Combine(_storagePath, serverFileName);
            if (!System.IO.File.Exists(filePath)) return null;
            return await System.IO.File.ReadAllBytesAsync(filePath);
        }

        public IEnumerable<Task<byte[]>>? GetFilesBitesByNote(string[] serverFileNames) => 
            serverFileNames.Select(GetFileBites);

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
            
            var path = Path.Combine(_storagePath, serverFileName);
            using (FileStream fs = System.IO.File.Create(path))
            {
                file.CopyTo(fs);
                return serverFileName;
            }
        }
    }
}
