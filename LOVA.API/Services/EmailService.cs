﻿using LOVA.API.Models;
using LOVA.API.Settings;
using LOVA.API.ViewModels;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            // TODO: Add dynamic sender list

            var senderList = await _context.MailSubscriptions
                .Include(a => a.MailType)
                .Where(a => a.MailType.Type == "NoActivitySinceEmail")
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
    }
}
