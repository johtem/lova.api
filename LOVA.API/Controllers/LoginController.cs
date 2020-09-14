using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Areas.Identity.Pages.Account;
using LOVA.API.Data;
using LOVA.API.Models;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LOVA.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;


        public LoginController(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }




        // POST api/<LoginController>
        [HttpPost]
        public async Task<string> Post([FromBody] UsersViewModel value)
        {
            //Check if user exist

            var result = await _signInManager.PasswordSignInAsync(value.UserName, value.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");


                return JsonConvert.SerializeObject("User logged in.");
            }

            else
            {
                return JsonConvert.SerializeObject("User/password not correct.");
            }

        }


    }
}
