using LOVA.API.Models;
using LOVA.API.Settings;
using LOVA.API.ViewModels;
using LOVA.API.ViewModels.Lova;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace LOVA.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        private readonly LovaDbContext _context;

        private readonly string contentRootPath;

        public EmailService(IOptions<MailSettings> emailSettings, IWebHostEnvironment env, LovaDbContext context)
        {
            _mailSettings = emailSettings.Value;
            contentRootPath = env.WebRootPath;
            _context = context;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }



        public async Task SendToManyActivitiesEmailAsync(ActivityCount request)
        {
            string FilePath = contentRootPath +
                Path.DirectorySeparatorChar.ToString()
                + "Templates"
                + Path.DirectorySeparatorChar.ToString()
                + "HighActivities.txt";

            // FilePath = "https://lottingelundfiles.blob.core.windows.net/emailtemplates/HighActivities.html";

            string MailText = string.Empty;
            using (StreamReader str = new StreamReader(FilePath))
            {
                MailText = str.ReadToEnd();
            };


            MailText = MailText.Replace("[adress]", request.Address)
                .Replace("[antalAktiveringar]", request.CountActivity.ToString())
                .Replace("[timme]", request.Hourly.ToString());
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse("johan@tempelman.nu"));
            email.Subject = $"Intagsenhet {request.Address}";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }


        public async Task SendNoActivitiesEmailAsync()
        {
            var weekAgo = DateTime.Now.AddDays(-6);

            IEnumerable<WellsDashboardViewModel> data = await _context.ActivityPerRows
                .Where(a => a.IsGroupAddress == false)
                .GroupBy(x => x.Address, (x, y) => new WellsDashboardViewModel
                {
                    Address = x,
                    Date = y.Max(z => z.TimeUp)
                })
                .OrderBy(n => n.Date)
                .Where(a => a.Date <= weekAgo)
                .ToListAsync();

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);

            // Dynamic sender list

            var senderList = await _context.MailSubscriptions
                .Include(a => a.MailType)
                .Where(a => a.MailType.Type == "NoActivitySinceEmail" && a.IsScription == true)
                .ToListAsync();


            foreach (var sender in senderList)
            {
                email.To.Add(MailboxAddress.Parse(sender.Email));
            }


            email.Subject = "Löva - Aktiviteter äldre än " + weekAgo;


            string textBody = "<br>";
            textBody += "<h5>Nedan tabell visar senaste aktivering(tömning) som är äldre än " + weekAgo + ".</h5><br>";
            textBody += "";
            textBody += " <table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + ">";
            textBody += "<tr bgcolor='#4da6ff'><td><b>Intagsenhet</b></td> <td> <b> Senaste aktivering</b> </td></tr>";
            foreach (var item in data)
            {
                textBody += "<tr><td>" + item.Address + "</td><td> " + item.Date + "</td> </tr>";
            }
            textBody += "</table><br><br>";
            textBody += "Automatiskt mailutskick varje måndag från www.lottingelund.se";


            var builder = new BodyBuilder();
            builder.HtmlBody = textBody;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);

        }

       

        public async Task SendAlarmEmailAsync(MailRequest mailRequest, string emailType)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);

            // Dynamic sender list

            var senderList = await _context.MailSubscriptions
                .Include(a => a.MailType)
                .Where(a => a.MailType.Type == emailType && a.IsScription == true)
                .ToListAsync();

            foreach (var sender in senderList)
            {
                email.To.Add(MailboxAddress.Parse(sender.Email));
            }
            //  email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));

            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendMaintenenceReminderEmailAsync()
        {

            var oneMonthFromNow = DateTime.Now.AddMonths(1);

            IEnumerable<MaintenanceViewModel> data = await _context.LatestMaintenances
                .Where(a => a.NextMaintenance <= oneMonthFromNow)
                .Include(a => a.Maintenance)
                .ThenInclude(a => a.MaintenanceGroup)
                .ThenInclude(a => a.Association)
                .OrderByDescending(a => a.NextMaintenance)
                .Select(x => new MaintenanceViewModel
                {
                    Name = x.Maintenance.Name,
                    MaintenanceGroup = x.Maintenance.MaintenanceGroup.GroupName,
                    LastMaintenance = x.LastMaintenance,
                    RecurringFrequence = x.Maintenance.RecurringFrequence,
                    NextMaintenance = x.NextMaintenance,
                    Association = x.Maintenance.Association.ShortName

                })
                .AsNoTracking()
                .ToListAsync();


            //IEnumerable<MaintenanceViewModel> data =  await _context.Maintenances
            //    .Include(a => a.LatestMaintenances)

            //    .Select(a => new MaintenanceViewModel
            //    {
            //        Id = a.Id,
            //        LastMaintenance = a.LatestMaintenances.Max(d => d.LastMaintenance),
            //        Association = a.Association,
            //        MaintenanceGroup = a.MaintenanceGroup,
            //        Name = a.Name,
            //        RecurringFrequence = a.RecurringFrequence,
            //        NextMaintenance = a.LatestMaintenances.Max(d => d.NextMaintenance)
            //    })
            //    .OrderBy(a => a.NextMaintenance)
            //    .ToListAsync();


            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);

            // Dynamic sender list

            var senderList = await _context.MailSubscriptions
                .Include(a => a.MailType)
                .Where(a => a.MailType.Type == "MaintenanceReminderEmail" && a.IsScription == true)
                .ToListAsync();


            foreach (var sender in senderList)
            {
                email.To.Add(MailboxAddress.Parse(sender.Email));
            }


            email.Subject = "Löva - Underhållsaktiviteter ";


            string textBody = "<br>";
            textBody += "<h3>Nedan tabell visar underhållsaktiviter som snart måste göras .</h3><br>";
            textBody += "";
            textBody += " <table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + ">";
            textBody += "<tr bgcolor='#4da6ff'><td><b>Underhållsaktivitet</b></td> <td> <b> Senaste </b> </td><td> <b> Nästa </b> </td></tr>";
            foreach (var item in data)
            {
                textBody += "<tr><td>" + item.Name + "</td><td> " + item.LastMaintenance + "</td><td> " + item.NextMaintenance + "</td> </tr>";
            }
            textBody += "</table><br><br>";
            textBody += "Automatiskt mailutskick varje natt från www.lottingelund.se";


            var builder = new BodyBuilder();
            builder.HtmlBody = textBody;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
