using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class UploadedFile
    {
        public long Id { get; set; }
        public string FileName { get; set; }

        public string  Directory { get; set; }
        public string Path { get; set; }
        public long UploadFileCategoryId { get; set; }
        public long UploadFileDirectoryId { get; set; }

        public string AspNetUserId { get; set; }

        public bool HasDirectories { get; set; }
        public bool IsDirectory { get; set; }

        public long Size { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
