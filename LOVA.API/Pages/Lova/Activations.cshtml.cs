using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using LOVA.API.Models;
using LOVA.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;

namespace LOVA.API.Pages.Lova
{
    public class ActivationsModel : PageModel
    {

        public IEnumerable<DrainTableStorageEntity> qResult { get; set; }
        public async Task OnGet()
        {
            // Create reference an existing table
            CloudTable table = await TableStorageCommon.CreateTableAsync("Drains");
            
            qResult = await TableStorageUtils.GetAllAsync(table);

            ViewData["qResult"] = JsonConvert.SerializeObject(qResult);


        }
    }
}
