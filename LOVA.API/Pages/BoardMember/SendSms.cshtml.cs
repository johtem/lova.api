using LOVA.API.Data;
using LOVA.API.Models;
using LOVA.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace LOVA.API.Pages.BoardMember
{
    public class SendSmsModel : PageModel
    {
        private readonly ITwilioRestClient _client;
        private readonly LovaDbContext _context;
        private readonly LOVAAPIContext _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SendSmsModel(ITwilioRestClient client, LovaDbContext context, LOVAAPIContext userManager, RoleManager<IdentityRole> roleManager)
        {
            _client = client;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public SmsMessage Message { get; set; } = new SmsMessage();


        [BindProperty]
        public IList<PremiseContact> Smslist { get; set; } = new List<PremiseContact>();

        // public string Test { get; set; }

        // public IEnumerable<ApplicationUser> Users { get; set; }

        public void OnGet()
        {

            Message.ListType = "Styrelse";
            //Users = await _userManager.Users.Where(a => a.PhoneNumber != null).ToListAsync();


            //foreach(var item in Users)
            //{
            //    var phoneNumber = item.PhoneNumber;
            //    Test = phoneNumber;
            //}


        }



        public async Task OnPostGetSelectedPersons()
        {
            var roles = _roleManager.Roles.Where(a => a.Name == Message.ListType);

            // var smslist = new List<PremiseContact>();



            if (Message.ListType != "User")
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
                        .Where(a => a.IsDeleted == false && a.IsActive == true && a.WantInfoSMS == true && a.Email == item.UserName)
                        .FirstOrDefaultAsync();
                    if (temp != null)
                    {
                        Smslist.Add(temp);
                    }

                }
            }
            else
            {
                if (Message.IsNode1 == true && Message.IsNode2 == true && Message.IsNode3 == false)
                {
                    Smslist = _context.PremiseContacts
                        .Where(a => a.IsDeleted == false && a.IsActive == true && a.WantInfoSMS == true && a.Premise.Well.MasterNode != 3).ToList();
                }
                else if (Message.IsNode1 == true && Message.IsNode2 == false && Message.IsNode3 == false)
                {
                    Smslist = _context.PremiseContacts
                        .Where(a => a.IsDeleted == false && a.IsActive == true && a.WantInfoSMS == true && a.Premise.Well.MasterNode == 1).ToList();
                }
                else if (Message.IsNode1 == false && Message.IsNode2 == true && Message.IsNode3 == true)
                {
                    Smslist = _context.PremiseContacts
                        .Where(a => a.IsDeleted == false && a.IsActive == true && a.WantInfoSMS == true && a.Premise.Well.MasterNode != 1).ToList();
                }
                else if (Message.IsNode1 == false && Message.IsNode2 == true && Message.IsNode3 == false)
                {
                    Smslist = _context.PremiseContacts
                        .Where(a => a.IsDeleted == false && a.IsActive == true && a.WantInfoSMS == true && a.Premise.Well.MasterNode == 2).ToList();
                }
                else if (Message.IsNode1 == true && Message.IsNode2 == false && Message.IsNode3 == true)
                {
                    Smslist = _context.PremiseContacts
                        .Where(a => a.IsDeleted == false && a.IsActive == true && a.WantInfoSMS == true && a.Premise.Well.MasterNode != 2).ToList();
                }
                else if (Message.IsNode1 == false && Message.IsNode2 == false && Message.IsNode3 == true)
                {
                    Smslist = _context.PremiseContacts
                        .Where(a => a.IsDeleted == false && a.IsActive == true && a.WantInfoSMS == true && a.Premise.Well.MasterNode == 3).ToList();
                }
                else
                {
                    Smslist = _context.PremiseContacts.Where(a => a.IsDeleted == false && a.IsActive == true && a.WantInfoSMS == true).ToList();
                }



            }


            //Smslist.Clear();
            //Smslist.Add(new PremiseContact
            //{
            //    Id = 2,
            //    PremiseId = 18,
            //    FirstName = "Johan",
            //    LastName = "Tempelman",
            //    MobileNumber = "0734435407",
            //    PhoneNumber = "",
            //    IsActive = true,
            //    WantGrannsamverkanEmail = true,
            //    WantInfoEmail = true,
            //    WantInfoSMS = true

            //});
        }

        public async Task OnPost()
        {

            Message.From = "+46723499120";

            var numberConvert = new PhoneNumberConverter();

            // var smslist = new List<PremiseContact>();

            foreach (var sms in Smslist)
            {
                if (sms.WantInfoSMS)
                {
                    var message = MessageResource.Create(
                        to: new PhoneNumber(numberConvert.ConvertPhoneNumber(sms.MobileNumber)),
                        from: new PhoneNumber(Message.From),
                        body: Message.Message,
                        client: _client); // pass in the custom client
                }

            }





        }
    }
}
