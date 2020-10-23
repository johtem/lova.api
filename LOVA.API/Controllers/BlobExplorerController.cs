using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Models;
using LOVA.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LOVA.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]  //Remove from Swagger page
    [Route("blobs")]
  public class BlobExplorerController : Controller
    {
        private readonly IBlobService _blobService;

        public BlobExplorerController(IBlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpGet("{blobName}")]
        public async Task<IActionResult> GetBlob(string blobName)
        {
            var data = await _blobService.GetBlobAsync(blobName, "lottingelundfiles");
            return File(data.Content, data.ContentType);
        }

        [HttpGet("list")]

        public async Task<IActionResult> ListBlobs()

        {

            return Ok(await _blobService.ListBlobsAsync());

        }



        [HttpPost("uploadfile")]

        public async Task<IActionResult> UploadFile([FromBody] UploadFileRequest request)

        {

            await _blobService.UploadFileBlobAsync(request.FilePath, request.FileName, "");

            return Ok();

        }



        [HttpPost("uploadcontent")]

        public async Task<IActionResult> UploadContent([FromBody] UploadContentRequest request)

        {

            await _blobService.UploadContentBlobAsync(request.Content, request.FileName);

            return Ok();

        }



        [HttpDelete("{blobName}")]

        public async Task<IActionResult> DeleteFile(string blobName)

        {

            await _blobService.DeleteBlobAsync(blobName);

            return Ok();

        }

    }
}