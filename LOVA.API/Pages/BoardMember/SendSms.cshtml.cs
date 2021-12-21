using LOVA.API.Models;
using LOVA.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace LOVA.API.Pages.BoardMember
{
    public class SendSmsModel : PageModel
    {
        private readonly ITwilioRestClient _client;
        private readonly LovaDbContext _context;

        public SendSmsModel(ITwilioRestClient client, LovaDbContext context)
        {
            _client = client;
            _context = context;
        }

        [BindProperty]
        public SmsMessage Message { get; set; }

        public void OnGet()
        {

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
