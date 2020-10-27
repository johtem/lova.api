using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LOVA.API.Extensions;
using LOVA.API.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOVA.API.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task DeleteBlobAsync(string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(blobName);
            var blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.DeleteIfExistsAsync();

        }

        public async Task<Models.BlobInfo> GetBlobAsync(string name, string containerName = "lottingelundfiles")
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(name);
            var blobDownloadInfo = await blobClient.DownloadAsync();

            return new Models.BlobInfo(blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType);
        }

        public async Task<IEnumerable<string>> ListBlobsAsync()
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("lottingelundfiles");
            var items = new List<string>();

            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                items.Add(blobItem.Name);
            }

            return items;

        }



        public async Task UploadFileBlobAsync(IFormFile file, string containerName = "lottingelundfiles")
        {
            // bool isSaveSuccess = false;
            string fileName;


            try
            {
                var fileContent = Microsoft.Net.Http.Headers.ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));

                var localFilePath = Path.GetTempFileName();

                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync();

                BlobClient blobClient = containerClient.GetBlobClient(fileName);


                // Open the file and upload its data
                using Stream uploadFileStream = file.OpenReadStream();
                await blobClient.UploadAsync(uploadFileStream, new BlobHttpHeaders { ContentType = fileName.GetContentType() });
                uploadFileStream.Close();


            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}
