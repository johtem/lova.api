using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Models;
using LOVA.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOVA.API.Pages
{
    public class TestEmailModel : PageModel
    {
        private readonly IEmailService _emailService;

        public TestEmailModel(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public void OnGet()
        {
            
            var message = new EmailMessage
            {
                ToAddresses = new List<EmailAddress> {
                    new EmailAddress { Name = "Johan", Address = "johan.tempelman@bt.com"}
                },                 
                FromAddresses = new List<EmailAddress> {
                    new EmailAddress { Name = "Johan", Address = "johan@tempelman.nu"}
                },
                Content = "This is a test email",
                Subject = "Löttinglund test mail"
            };

            _emailService.Send(message);
        }
    }
}