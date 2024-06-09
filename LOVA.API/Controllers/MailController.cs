using LOVA.API.Hubs;
using LOVA.API.Models;
using LOVA.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IEmailService mailService;

        private readonly Microsoft.AspNetCore.SignalR.IHubContext<ActivationHub> _hub;
        public MailController(IEmailService mailService, IHubContext<ActivationHub> hub)
        {
            this.mailService = mailService;
            _hub = hub;

        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            try
            {
                await mailService.SendEmailAsync(request);
                return Ok();
            }
            catch
            {
                throw;
            }

        }


        [HttpPost("welcome")]
        public async Task<IActionResult> SendWelcomeMail([FromForm] ActivityCount request)
        {
            try
            {
                //await mailService.SendToManyActivitiesEmailAsync(request);
                await mailService.SendNoActivitiesEmailAsync();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("NoActivitiesEmail")]
        public async Task<IActionResult> SendNoActivitiesEmailAsync()
        {
            try
            {
                
                 await mailService.SendNoActivitiesEmailAsync();
                 
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        public async Task Get()
        {

            var insertData = new Activity
            {
                Master_node = 3,
                Index = 2,
                Address = "3C1",
                Time = DateTime.Now,
                Active = true
            };
                


            await _hub.Clients.All.SendAsync("ReceiveMessage", insertData.Address, insertData);
        }






    }
}
