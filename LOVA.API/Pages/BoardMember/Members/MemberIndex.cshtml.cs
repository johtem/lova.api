using LOVA.API.Services;
using LOVA.API.ViewModels.AddressList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Pages.BoardMember.Members
{
    public class MemberIndexModel : PageModel
    {
        private readonly LOVA.API.Services.LovaDbContext _context;

        public MemberIndexModel(LovaDbContext context)
        {
            _context = context;
        }

        public IList<ContactIndexViewModel> Members { get; set; }

        public async Task OnGet()
        {
            Members = await _context.PremiseContacts
                .Include(a => a.Premise)
                .ThenInclude(b => b.Well)
                .AsNoTracking()
                .Select( a => new ContactIndexViewModel
                {
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Address = a.Premise.Address,
                    WellName = a.Premise.Well.WellName,
                    Email = a.Email,
                    MobileNumber = a.MobileNumber,
                    PhoneNumber = a.PhoneNumber,
                    PremiseContactId = a.Id,
                    IsActive = a.IsActive,
                    Property = a.Premise.Property,
                    SortDate = a.UpdatedAt
                })
                .OrderByDescending(a => a.SortDate)
                .ToListAsync();



        }
    }
}
