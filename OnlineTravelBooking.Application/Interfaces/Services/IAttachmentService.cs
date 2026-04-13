using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Interfaces.Services
{
    public interface IAttachmentService
    {
        //Upload file to server and return the file path
        public string? Upload(IFormFile file, string folderName);
        //Delete file from server
        public bool Delete(string filePath);
    }
}
