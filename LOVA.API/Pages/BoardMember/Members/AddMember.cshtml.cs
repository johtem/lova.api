using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels.AddressList;

namespace LOVA.API.Pages.BoardMember.Members
{
    public class AddMemberModel : PageModel
    {
        private readonly LOVA.API.Services.LovaDbContext _context;

        public AddMemberModel(LOVA.API.Services.LovaDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["PremiseId"] = new SelectList(_context.Premises.OrderBy(a => a.Address), "Id", "Address");
            return Page();
        }

        [BindProperty]
        public ContactViewModel Contact { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var now = DateTime.Now;

            var premiseContact = new PremiseContact
            {
                FirstName = Contact.FirstName,
                LastName = Contact.LastName,
                PhoneNumber = Contact.PhoneNumber,
                MobileNumber = Contact.MobileNumber,
                Email = Contact.Email,
                PremiseId = Contact.PremiseId,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
         
            };

            _context.PremiseContacts.Add(premiseContact);
            await _context.SaveChangesAsync();

            return RedirectToPage("./MemberIndex");
        }
    }
}
