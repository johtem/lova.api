﻿using LOVA.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace LOVA.API.Data
{
    // public class LOVAAPIContext : IdentityUserContext<ApplicationUser>
    public class LOVAAPIContext : IdentityDbContext<ApplicationUser> 
    {
        public LOVAAPIContext(DbContextOptions<LOVAAPIContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

          
            
        }
    }
}
