using LOVA.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Services
{
    public class LovaDbContext : DbContext
    {
        public LovaDbContext(DbContextOptions<LovaDbContext> options) : base(options)
        {

        }

        public DbSet<Premise> Premises { get; set; }
        public DbSet<Well> Wells { get; set; }
        public DbSet<IssueReport> IssueReports { get; set; }

        public DbSet<DrainPatrol> DrainPatrols { get; set; }


    }
}
