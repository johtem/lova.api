using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Extensions;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages.Lottingelund
{
    public class DocumentVAModel : PageModel
    {
        private readonly LovaDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBlobService _blobService;

        public DocumentVAModel(LovaDbContext context, UserManager<ApplicationUser> userManager, IBlobService blobService)
        {
            _context = context;
            _userManager = userManager;
            _blobService = blobService;
        }

        public void OnGet()
        {
        }


        public async Task<JsonResult> OnPostRead(string target)
        {
            //target = "Löttingelundsbladet";

            var lovaDirectories = await _context.UploadFileDirectories
                .Where(a => a.UploadFileCategoryId == 6 && string.IsNullOrEmpty(target) ? a.Directory != target : a.Directory == "apa")
                .Select(a => new ViewFilesViewModel
                {
                    Name = a.Directory,
                    Path = a.Directory,
                    Size = 0,
                    Created = DateTime.Now,
                    CreatedUtc = DateTime.Now,
                    Modified = DateTime.Now,
                    ModifiedUtc = DateTime.Now,
                    HasDirectories = false,
                    IsDirectory = true,
                    Extension = ""
                })
                .ToArrayAsync();

            var lovaFiles = await _context.UploadedFiles
                .Where(a => a.UploadFileCategoryId == 6 && a.IsDirectory == false && a.Directory == target)
                .Select(a => new ViewFilesViewModel
                {
                    Name = Path.GetFileNameWithoutExtension(a.FileName),
                    Path = a.Path,
                    Size = a.Size,
                    Created = a.CreatedAt,
                    CreatedUtc = a.CreatedAt,
                    Modified = a.UpdatedAt,
                    ModifiedUtc = a.UpdatedAt,
                    HasDirectories = a.HasDirectories,
                    IsDirectory = a.IsDirectory,
                    Extension = Path.GetExtension(a.FileName)
                })
                .ToArrayAsync();

            var files = lovaDirectories.Concat(lovaFiles);


            return new JsonResult(files);
        }


        public async Task<IActionResult> OnGetDownload(string file)
        {

            var data = await _blobService.GetBlobAsync(file, "lottingelundfiles");

            return File(data.Content, file.GetContentType(), file);

        }
    }
}
