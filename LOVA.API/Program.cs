using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Extensions.AspNetCore.Configuration.Secrets;

namespace LOVA.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }


        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var env = context.HostingEnvironment;
                    var buildConfig = config.Build();
                    var vaultName = buildConfig["KeyVaultName"];

                    if (env.EnvironmentName != "Development")
                    {
                        Uri vaultUri = new Uri($"https://{vaultName}.vault.azure.net/");

                        // Load all secrets from the vault into configuration. This will automatically
                        // authenticate to the vault using a managed identity. If a managed identity
                        // is not available, it will check if Visual Studio and/or the Azure CLI are
                        // installed locally and see if they are configured with credentials that can
                        // access the vault.
                        config.AddAzureKeyVault(vaultUri, new DefaultAzureCredential());
                    }
                    

                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
