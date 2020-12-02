using LOVA.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.ViewModels;

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

        // public DbSet<DrainPatrol> DrainPatrols { get; set; }
        public DbSet<DrainPatrolAlarm> DrainPatrolAlarms { get; set; }

        public DbSet<RentalInventory> RentalInventories { get; set; }

        public DbSet<RentalReservation> RentalReservations { get; set; }

        public DbSet<UploadedFile> UploadedFiles{ get; set; }

        public DbSet<UploadFileCategory> UploadFileCategories { get; set; }

        public DbSet<UploadFileDirectory> UploadFileDirectories { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<Notification> Notifications { get; set; }




    }
}
