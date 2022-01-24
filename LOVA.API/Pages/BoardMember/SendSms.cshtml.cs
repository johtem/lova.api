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

        public SendSmsModel(ITwilioRestClient client, LovaDbContext context, LOVAAPIContext userManager)
        {
            _client = client;
            _context = context;
            _userManager = userManager;
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

        public void OnPost()
        {

            Message.From = "+46723499120";

            var numberConvert = new PhoneNumberConverter();

            Message.To = numberConvert.ConvertPhoneNumber(Message.To);


            var message = MessageResource.Create(
            to: new PhoneNumber(Message.To),
            from: new PhoneNumber(Message.From),
            body: Message.Message,
            client: _client); // pass in the custom client


        }
    }
}
