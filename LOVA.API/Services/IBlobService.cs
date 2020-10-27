using LOVA.API.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Services
{
    public interface IBlobService
    {
        public Task<BlobInfo> GetBlobAsync(string name, string containerName);

        public Task<IEnumerable<string>> ListBlobsAsync();


        public Task UploadFileBlobAsync(IFormFile file, string containerName);

       

        public Task DeleteBlobAsync(string blobName);
    }
}
