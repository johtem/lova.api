using System;
using LOVA.API.Data;
using LOVA.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(LOVA.API.Areas.Identity.IdentityHostingStartup))]
namespace LOVA.API.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<LOVAAPIContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("LOVAAPIContextConnection")));

                services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()    
                .AddEntityFrameworkStores<LOVAAPIContext>();

                services.Configure<IdentityOptions>(options =>
                {
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzåäöABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ0123456789-._@+: ";
                    options.User.RequireUniqueEmail = false;


                });


            });
        }
    }
}