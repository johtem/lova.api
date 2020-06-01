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
        public long UploadFileCategoryId { get; set; }
        public string AspNetUserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
