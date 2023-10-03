using LOVA.API.Data;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Twilio.Types;
using NPOI.SS.Formula.Functions;
using System.Text;
using EllipticCurve;
using System.Text.RegularExpressions;

namespace LOVA.API.Pages.BoardMember
{
    public class SendEmailModel : PageModel
    {
        private readonly LovaDbContext _context;
        private readonly LOVAAPIContext _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public SendEmailModel( LovaDbContext context, LOVAAPIContext userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }




        [BindProperty]
        public SmsMessage Message { get; set; } = new SmsMessage();

        [BindProperty]
        public string Sendlist { get; set; } 


        [BindProperty]
        public IList<PremiseContact> Emaillist { get; set; } = new List<PremiseContact>();


        public void OnGet()
        {

            Message.ListType = "Styrelse";

        }



        public async Task GetEmails()
        {
            var roles = _roleManager.Roles.Where(a => a.Name == Message.ListType);

            // var smslist = new List<PremiseContact>();

            Sendlist = Message.ListType;


            if (Message.ListType == "Styrelse" || Message.ListType == "Lova")
            {
                var users = await (from user in _userManager.Users
                                   join userRoles in _userManager.UserRoles on user.Id equals userRoles.UserId
                                   join role in _userManager.Roles on userRoles.RoleId equals role.Id
                                   where role.Name == Message.ListType
                                   select new { UserId = user.Id, UserName = user.UserName, RoleId = role.Id, RoleName = role.Name })
                        .ToListAsync();

                // var users = await _userManager.Users.Where(a => a.UserName == "johan@tempelman.nu" && a.PhoneNumber != null).ToListAsync();



                foreach (var item in users)
                {
                    var temp = await _context.PremiseContacts
                        .Include(a => a.Premise)
                        .Where(a => a.IsDeleted == false && a.IsActive == true && a.WantInfoEmail == true && a.Email == item.UserName)
                        .FirstOrDefaultAsync();
                    if (temp != null)
                    {
                        Emaillist.Add(temp);
                    }

                }
            }
            else if (Message.ListType == "Grannsamverkan")
            {
                Emaillist = _context.PremiseContacts
                    .Include(a => a.Premise)
                        .Where(a => a.IsDeleted == false && a.IsActive == true && a.WantGrannsamverkanEmail == true && a.Email != null).ToList();
            }
            else
            {
                if (Message.IsNode1 == true && Message.IsNode2 == true && Message.IsNode3 == false)
                {
                    Emaillist = _context.PremiseContacts
                        .Include(a => a.Premise)
                        .Where(a => a.IsDeleted == false && a.IsActive == true && a.WantInfoEmail == true && a.Email != null && a.Premise.Well.MasterNode != 3).ToList();
                }
                else if (Message.IsNode1 == true && Message.IsNode2 == false && Message.IsNode3 == false)
                {
                    Emaillist = _context.PremiseContacts
                        .Include(a => a.Premise)
                        .Where(a => a.IsDeleted == false && a.IsActive == true && a.WantInfoEmail == true && a.Email != null && a.Premise.Well.MasterNode == 1).ToList();
                }
                else if (Message.IsNode1 == false && Message.IsNode2 == true && Message.IsNode3 == true)
                {
                    Emaillist = _context.PremiseContacts
                        .Include(a => a.Premise)
                        .Where(a => a.IsDeleted == false && a.IsActive == true && a.WantInfoEmail == true && a.Email != null && a.Premise.Well.MasterNode != 1).ToList();
                }
                else if (Message.IsNode1 == false && Message.IsNode2 == true && Message.IsNode3 == false)
                {
                    Emaillist = _context.PremiseContacts
                        .Include(a => a.Premise)
                        .Where(a => a.IsDeleted == false && a.IsActive == true && a.WantInfoEmail == true && a.Email != null && a.Premise.Well.MasterNode == 2).ToList();
                }
                else if (Message.IsNode1 == true && Message.IsNode2 == false && Message.IsNode3 == true)
                {
                    Emaillist = _context.PremiseContacts
                        .Include(a => a.Premise)
                        .Where(a => a.IsDeleted == false && a.IsActive == true && a.WantInfoEmail == true && a.Email != null && a.Premise.Well.MasterNode != 2).ToList();
                }
                else if (Message.IsNode1 == false && Message.IsNode2 == false && Message.IsNode3 == true)
                {
                    Emaillist = _context.PremiseContacts
                        .Include(a => a.Premise)
                        .Where(a => a.IsDeleted == false && a.IsActive == true && a.WantInfoEmail == true && a.Email != null && a.Premise.Well.MasterNode == 3).ToList();
                }
                else
                {
                    Emaillist = _context.PremiseContacts
                        .Include(a => a.Premise)
                         .Where(a => a.IsDeleted == false && a.IsActive == true && a.Email != null && a.WantInfoEmail == true).ToList();
                }

            }

        }

        public async Task OnPost(IEnumerable<IFormFile> files)
        {
            // TODO: Add check for subject row and message text

            await GetEmails();

            var apiKey = _configuration.GetValue<string>("SendGridApiKey");
            var fromEmail = _configuration.GetValue<string>("FromEmail");
            var fromName = _configuration.GetValue<string>("FromName");

            if (string.IsNullOrEmpty(apiKey)) throw new Exception("SendGridApiKey should not be null or empty");
            if (string.IsNullOrEmpty(fromEmail)) throw new Exception("FromEmail should not be null or empty");
            if (string.IsNullOrEmpty(fromName)) throw new Exception("FromName should not be null or empty");


            var client = new SendGridClient(apiKey);

            var tos = new List<SendGrid.Helpers.Mail.EmailAddress>();

            foreach (var email in Emaillist)
            {
                if (email.WantInfoEmail)
                {
                    tos.Add(new SendGrid.Helpers.Mail.EmailAddress(email.Email, $"{email.FirstName} {email.LastName}"));                   
                }

            }

            //  Add a copy to info@lottingelund.se
            tos.Add(new SendGrid.Helpers.Mail.EmailAddress("info@lottingelund.se", $"Info Löttingelund"));


            var msg = new SendGridMessage()
                {
                    From = new SendGrid.Helpers.Mail.EmailAddress(fromEmail, fromName),
                    Subject = Message.Subject,
                    HtmlContent = Message.Message + "<br><span style=\"font-size: 8pt;\">Om du vill avsluta din prenumeration, <a href=\"https://www.lottingelund.se/User/ProfileIndex\" target=\"_blank\" rel=\"noopener\">klicka här</a>.</span>",
                    Personalizations = tos.Select(s => new Personalization
                    {
                        Tos = new List<SendGrid.Helpers.Mail.EmailAddress> { new SendGrid.Helpers.Mail.EmailAddress(s.Email, s.Name)},
                        Substitutions = new Dictionary<string, string>
                        {
                            {"-Namn-", s.Name }
                        }
                    }).ToList()
                };


                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var stream = new MemoryStream((int)file.Length);
                        file.CopyTo(stream);
                        var bytes = stream.ToArray();


                    string fileName = file.FileName.Replace("å", "a").Replace("ä", "a").Replace("ö", "o").Replace("Å", "A").Replace("Ä", "A").Replace("Ö", "O");


                        Attachment att = new Attachment
                        { 
                            Content = Convert.ToBase64String(bytes), 
                            Type = file.ContentType,
                            Filename = fileName,
                            Disposition = "attachment",
                            ContentId = fileName
                        };

                        msg.AddAttachment(att);
                    }
                };

    
                // Send message
                var response = await client.SendEmailAsync(msg);

            

            Sendlist = "";
            Emaillist.Clear();
            Message = new SmsMessage();

     

        }
 
    }
}
