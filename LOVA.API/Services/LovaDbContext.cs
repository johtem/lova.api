using LOVA.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.ViewModels;
using LOVA.API.Models.Lova;

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

        public DbSet<ActivityPerRow> ActivityPerRows { get; set; }

        public DbSet<ActivityCount> ActivityCounts { get; set; }

        public DbSet<MailType> MailTypes { get; set; }
        public DbSet<MailSubscription> MailSubscriptions { get; set; }

        public DbSet<FullDrain> FullDrains { get; set; }
        public DbSet<WellMaintenanceWork> WellMaintenanceWorks { get; set; }

        public DbSet<Survey> Surveys { get; set; }

        public DbSet<SurveyCheckbox> SurveyCheckBoxes { get; set; }

        public DbSet<SurveyAnswered> SurveyAnswereds { get; set; }

        public DbSet<LovaIssue> LovaIssues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<LovaIssue>()
                .Property(p => p.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (Status)Enum.Parse(typeof(Status), v));
        }

        public DbSet<LOVA.API.Models.Lova.Maintenance> Maintenances { get; set; }
        public DbSet<LOVA.API.Models.Lova.LatestMaintenance> LatestMaintenances { get; set; }
    }
}
