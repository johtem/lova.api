using LOVA.API.Models;
using LOVA.API.Services;
using Microsoft.AspNetCore.Mvc;
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
        public MailController(IEmailService mailService)
        {
            this.mailService = mailService;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            try
            {
                await mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("NoActivitiesEmail")]
        public async Task<IActionResult> SendNoActivitiesEmailAsync()
        {
            try
            {
                //await mailService.SendToManyActivitiesEmailAsync(request);
                await mailService.SendNoActivitiesEmailAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
