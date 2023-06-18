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
        public SmsMessage Message { get; set; }


        public string Test { get; set; }

        public IEnumerable<ApplicationUser> Users { get; set; }

        public async Task OnGetAsync()
        {
            Users = await _userManager.Users.Where(a => a.PhoneNumber != null).ToListAsync();

            
            foreach(var item in Users)
            {
                var phoneNumber = item.PhoneNumber;
                Test = phoneNumber;
            }

           
        }

        public async Task OnPost()
        {

            Message.From = "+46723499120";

            var numberConvert = new PhoneNumberConverter();

            var roles = _roleManager.Roles.Where(a => a.Name == Message.ListType);

            var smslist = new List<PremiseContact>();
            
            
            if (Message.ListType != "User")
            {
                var users = await (from user in _userManager.Users
                                   join userRoles in _userManager.UserRoles on user.Id equals userRoles.UserId
                                   join role in _userManager.Roles on userRoles.RoleId equals role.Id
                                   where role.Name == Message.ListType
                                   select new { UserId = user.Id, UserName = user.UserName, RoleId = role.Id, RoleName = role.Name })
                        .ToListAsync();

                foreach (var item in users)
                {
                    var temp = await _context.PremiseContacts
                        .Where(a => a.IsDeleted == false && a.IsActive == true && a.WantInfoSMS == true && a.Email == item.UserName)
                        .FirstOrDefaultAsync();
                    if (temp != null)
                    {
                       smslist.Add(temp);
                    }
                    
                }
            }
            else
            {
                smslist = _context.PremiseContacts.Where(a => a.IsDeleted == false && a.IsActive == true && a.WantInfoSMS == true).ToList();
            }
                 

            foreach(var sms in smslist)
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
