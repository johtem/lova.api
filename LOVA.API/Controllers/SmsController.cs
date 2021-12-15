using LOVA.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.Types;
using LOVA.API.Services;
using System.Threading.Tasks;

namespace LOVA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : TwilioController
    {
        private readonly ITwilioRestClient _client;
        private readonly LovaDbContext _context;

        public SmsController(ITwilioRestClient client, LovaDbContext context)
        {
            _client = client;
            _context = context;
        }

        [HttpGet]
        public IActionResult SendSms(SmsMessage model)
        {
            var message = MessageResource.Create(
            to: new PhoneNumber(model.To),
            from: new PhoneNumber(model.From),
            body: model.Message,
            client: _client); // pass in the custom client
            return Ok("Success");
        }

        [HttpPost]
        public async Task<TwiMLResult> ReceiveSms([FromForm] SmsRequest incomingMessage)
        {
         

            var sms = new IncommingSms
            {
                To = incomingMessage.To,
                From = incomingMessage.From,
                Message = incomingMessage.Body,
                CreatedAt = System.DateTime.Now
            };

            await _context.IncommingSms.AddAsync(sms);

            await _context.SaveChangesAsync();


           
            var messagingResponse = new MessagingResponse();
            messagingResponse.Message($"Tack för din kommentar!");
            return TwiML(messagingResponse);
        }


    }
}
