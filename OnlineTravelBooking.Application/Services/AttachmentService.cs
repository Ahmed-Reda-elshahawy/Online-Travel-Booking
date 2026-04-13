using Microsoft.AspNetCore.Http;
using OnlineTravelBooking.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Services
{
    public class AttachmentService : IAttachmentService
    {
        string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        public string? Upload(IFormFile file, string folderName)
        {
            //1. Check the extension
            var extension = Path.GetExtension(file.FileName);

            if (!allowedExtensions.Contains(extension.ToLower()))
            {
                return null;
            }
            //2. check the size
            if (file.Length > 2 * 1024 * 1024 || file.Length == 0)
            {
                return null;
            }
            //3. Get located folder path
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", folderName);
            //4. Get the file name (unique name)
            var fileName = Guid.NewGuid().ToString() + file.FileName;
            //5. Get the full path
            var filePath = Path.Combine(folderPath, fileName);
            //6. create file stream
            using FileStream fileStream = new FileStream(filePath, FileMode.Create);
            //7. copy the file to file stream
            file.CopyTo(fileStream);
            //8. return the fileName
            return fileName;

        }
        public bool Delete(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }
    }
}
