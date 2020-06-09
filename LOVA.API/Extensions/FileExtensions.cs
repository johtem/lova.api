using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Extensions
{
    public static class FileExtensions
    {
        private static readonly FileExtensionContentTypeProvider Provider = new FileExtensionContentTypeProvider();

        public static string GetContentType(this string fileName)
        {
            //string extension = Path.GetExtension(fileName);

            //string contentType = "";

            //contentType = extension switch
            //{
            //    ".pdf" => "application/pdf",
            //    ".xslx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            //    ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            //    // and so on
            //    _ => "application/octet-stream",
            //};


            if (!Provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }
}
