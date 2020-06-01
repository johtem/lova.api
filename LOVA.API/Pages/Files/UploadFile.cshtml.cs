using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Extensions;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages.Files
{
    [Authorize(Roles = "Admin, Styrelse")]
    public class UploadFileModel : PageModel
    {
        private readonly IBlobService _blobService;
        private readonly LovaDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UploadFileModel(IBlobService blobService, LovaDbContext context, UserManager<ApplicationUser> userManager)
        {
            _blobService = blobService;
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public UploadFileViewModel FileUpload { get; set; }

        public SelectList Categories { get; set; }

        public void OnGet()
        {
            Categories = new SelectList(_context.UploadFileCategories, nameof(UploadFileCategory.Id), nameof(UploadFileCategory.Category));
        }

        public async Task OnPost(IFormFile files)
        {
            if (files != null)
            {
                var fileContent = Microsoft.Net.Http.Headers.ContentDispositionHeaderValue.Parse(files.ContentDisposition);
                var fileName = System.IO.Path.GetFileName(fileContent.FileName.ToString().Trim('"'));

                await _blobService.UploadFileBlobAsync(fileContent.FileName.ToString().Trim('"'), fileName);

                var user = await _userManager.GetUserAsync(HttpContext.User);

                var insertData = new UploadedFile
                {
                    FileName = fileName,
                    UploadFileCategoryId = FileUpload.UploadFileCategoryId,
                    CreatedAt = DateTime.Now,
                    AspNetUserId = user.Id

                };

                _context.UploadedFiles.Add(insertData);
                await _context.SaveChangesAsync();
            }



        }
    }
}