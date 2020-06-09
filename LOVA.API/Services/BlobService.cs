using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LOVA.API.Extensions;
using LOVA.API.Models;
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
            var containerClient = _blobServiceClient.GetBlobContainerClient("lottingelundfiles");
            var blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.DeleteIfExistsAsync();

        }

        public async Task<Models.BlobInfo> GetBlobAsync(string name)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("lottingelundfiles");
            var blobClient = containerClient.GetBlobClient(name);
            var blobDownloadInfo =  await blobClient.DownloadAsync();

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

        public async Task UploadContentBlobAsync(string content, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("lottingelundfiles");
            var blobClient = containerClient.GetBlobClient(fileName);

            var bytes = Encoding.UTF8.GetBytes(content);
            using var memoryStream = new MemoryStream(bytes);
            await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders { ContentType = fileName.GetContentType() });
        }

        public async Task UploadFileBlobAsync(string filePath, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("lottingelundfiles");
           
            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(filePath, new BlobHttpHeaders { ContentType = filePath.GetContentType() });

            

        }
    }
}
