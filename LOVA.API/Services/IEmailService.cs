using LOVA.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailRequest);

        Task SendToManyActivitiesEmailAsync(ActivityCount request);

        Task SendNoActivitiesEmailAsync();

        Task SendEmailLongActivationTime();
    }
}
