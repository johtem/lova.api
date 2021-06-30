using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using LOVA.API.Extensions;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LOVA.API.Pages.BoardMember
{
    [Authorize(Roles = "Admin, Styrelse")]
    public class DocumentsModel : PageModel
    {
        private readonly IBlobService _blobService;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly LovaDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public DocumentsModel(IBlobService blobService, BlobServiceClient blobServiceClient, LovaDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _blobService = blobService;
            _blobServiceClient = blobServiceClient;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public string Message { get; set; }

        public async Task<JsonResult> OnPostRead(string target)
        {
            //target = "Löttingelundsbladet";

            var lovaDirectories = await _context.UploadFileDirectories
                .Where(a => a.UploadFileCategoryId == 2 && string.IsNullOrEmpty(target) ? a.Directory != target : a.Directory == "apa")
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
                .Where(a => a.UploadFileCategoryId == 2 && a.IsDirectory == false && a.Directory == target)
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


        public async Task<JsonResult> OnPostUpload(string path, IFormFile file)

        {
            if (file != null)
            {
                var fileContent = Microsoft.Net.Http.Headers.ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                var fileName = System.IO.Path.GetFileName(fileContent.FileName.ToString().Trim('"'));

                var filePath = Path.GetTempFileName();

                var directory = await _context.UploadFileDirectories
                    .Where(a => a.Directory == path)
                    .FirstOrDefaultAsync();

                long size = file.Length;

                //  await UploadFileBlobAsync(filePath, fileName);

                string result = await UploadFile(file, filePath);

                if (result == "OK")
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);

                    var fileExit = await _context.UploadedFiles.Where(o => o.FileName == fileName && o.Directory == path && o.Container == MyConsts.boardDocuments).FirstOrDefaultAsync();

                    if (fileExit != null)
                    {
                        fileExit.UpdatedAt = DateTime.Now;
                        fileExit.AspNetUserId = user.Id;

                        _context.UploadedFiles.Add(fileExit);
                        _context.Entry(fileExit).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        var insertData = new UploadedFile
                        {
                            FileName = fileName,
                            Path = directory.Directory + "/" + fileName,
                            Size = size,
                            Directory = directory.Directory,
                            Container = MyConsts.boardDocuments,
                            UploadFileDirectoryId = directory.Id,
                            UploadFileCategoryId = directory.UploadFileCategoryId,
                            HasDirectories = false,
                            IsDirectory = false,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            AspNetUserId = user.Id

                        };

                        _context.UploadedFiles.Add(insertData);
                        await _context.SaveChangesAsync();
                    }
                }

                Message = fileName + " är nu sparad.";
            }


            var lovaDirectories = await _context.UploadFileDirectories
                .Where(a => a.UploadFileCategoryId == 2 && string.IsNullOrEmpty(path) ? a.Directory != path : a.Directory == "apa")
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
                .Where(a => a.UploadFileCategoryId == 2 && a.IsDirectory == false && a.Directory == path)
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


        public async Task<string> UploadFile(IFormFile file, string filePath)
        {

            try
            {
                await _blobService.UploadFileBlobAsync(file, MyConsts.boardDocuments);
            }
            catch (Exception)
            {

                return "ERROR";
            }
            

            return "OK";
        }


        public async Task<IActionResult> OnGetDownload(string file)
        {

            var data = await _blobService.GetBlobAsync(file, MyConsts.boardDocuments);

            return File(data.Content, file.GetContentType(), file);

        }
    }
}
