﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Kendo.Mvc.UI;
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
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LOVA.API.Pages.Files
{
    [Authorize(Roles = "Admin, Styrelse")]
    public class UploadFileModel : PageModel
    {
        private readonly IBlobService _blobService;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly LovaDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public UploadFileModel(IBlobService blobService, LovaDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration, BlobServiceClient blobServiceClient)
        {
            _blobService = blobService;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _blobServiceClient = blobServiceClient;
        }

        [BindProperty]
        public UploadFileViewModel FileUpload { get; set; }

        public string Message { get; set; }


        public async Task<JsonResult> OnGetAssociation([DataSourceRequest] DataSourceRequest request)
        {
            var associations = await _context.UploadFileCategories
                .Select( c => new DropdownViewModel()
                {
                    Id = c.Id,
                    Name = c.Category
                }).ToListAsync();

            return new JsonResult(associations);
        }

        public async Task<JsonResult> OnGetDocuments([DataSourceRequest] DataSourceRequest request, long? categoryId)
    
        {
            var documents = await _context.UploadFileDirectories
                .Where(a => a.UploadFileCategoryId == categoryId)
                .Select(c => new DropdownCascadedViewModel()
                {
                    CascadedId = c.Id,
                    CascadedName = c.Directory
                }).ToListAsync();

            return new JsonResult(documents);
        }

        public async Task OnPost(IFormFile files)
        {
            if (files != null)
            {
                var fileContent = Microsoft.Net.Http.Headers.ContentDispositionHeaderValue.Parse(files.ContentDisposition);
                var fileName = System.IO.Path.GetFileName(fileContent.FileName.ToString().Trim('"'));

                var filePath = Path.GetTempFileName();

                var directory = await _context.UploadFileDirectories
                    .Where(a => a.Id == FileUpload.UploadFileDirectoryId)
                    .FirstOrDefaultAsync();

                long size = files.Length;

                //  await UploadFileBlobAsync(filePath, fileName);

                // string container = FileUpload.UploadFileCategoryId == 1 ? MyConsts.lovaDocuments : MyConsts.lottingelundDocuments;

                string container = "";

                switch (FileUpload.UploadFileCategoryId)
                {
                    case 1:
                        container = MyConsts.lovaDocuments;
                        break;
                    case 2:
                        container = MyConsts.boardDocuments;
                        break;
                    default:
                        container = MyConsts.lottingelundDocuments;
                        break;
                }



                string result = await UploadFile(files, filePath, container);

                

                if (result == "OK")
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);

                    var fileExit = await _context.UploadedFiles.Where(o => o.FileName == fileName  && o.Directory == directory.Directory && o.Container == container).FirstOrDefaultAsync();

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
                            UploadFileDirectoryId = FileUpload.UploadFileDirectoryId,
                            UploadFileCategoryId = FileUpload.UploadFileCategoryId,
                            Container = container,
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
        }


        public async Task<string> UploadFile(IFormFile file, string filePath, string container)
        {

            try
            {
                await _blobService.UploadFileBlobAsync(file, container);
            }
            catch (Exception)
            {

                return "ERROR";
            }


            return "OK";

        }

           

        //public async Task UploadFileBlobAsync(string filePath, string fileName)
        //{
        //    var containerClient = _blobServiceClient.GetBlobContainerClient("lottingelundfiles");

        //    var blobClient = containerClient.GetBlobClient(fileName);

        //    await blobClient.UploadAsync(filePath, new BlobHttpHeaders { ContentType = fileName.GetContentType() });



        //}

    }
}