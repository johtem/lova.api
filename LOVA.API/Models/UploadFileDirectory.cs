using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class UploadFileDirectory
    {
        public long Id { get; set; }

        public long UploadFileCategoryId { get; set; }
        public UploadFileCategory UploadFileCategory { get; set; }
        public string Directory { get; set; }
    }
}
