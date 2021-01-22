using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace LOVA.API.Pages
{
    public class TestConfigModel : PageModel
    {

        private readonly IConfiguration _configuration;

        public TestConfigModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string apa { get; set; }

        public void OnGet()
        {
            //var myKeyValue = Configuration["MyKey"];
            //var title = Configuration["Position:Title"];
            //var name = Configuration["Position:Name"];
           // var defaultLogLevel = Configuration["Logging:LogLevel:Default"];
           var FromAzure = _configuration["SecretPassword"];



        //    SecretClientOptions options = new SecretClientOptions()
        //    {
        //        Retry =
        //{
        //    Delay= TimeSpan.FromSeconds(2),
        //    MaxDelay = TimeSpan.FromSeconds(16),
        //    MaxRetries = 5,
        //    Mode = RetryMode.Exponential
        // }
        //    };
        //    var client = new SecretClient(new Uri("https://kv-lova.vault.azure.net/"), new DefaultAzureCredential(), options);

        //    KeyVaultSecret secret = client.GetSecret("SecretPassword");

        //    string secretValue = secret.Value;


            apa = FromAzure;
        }
    }
}
