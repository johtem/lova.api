using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.ViewModels
{
    public class ViewFilesViewModel
    {

        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public DateTime Created { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string Extension { get; set; }
        public bool HasDirectories { get; set; }
        public bool IsDirectory { get; set; }

        public DateTime Modified { get; set; }

        public DateTime ModifiedUtc { get; set; }
    }
}
