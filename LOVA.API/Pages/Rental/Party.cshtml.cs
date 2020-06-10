using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages.Rental
{
    [Authorize(Roles = "User, Admin, Styrelse")]
    public class PartyModel : PageModel
    {
        private readonly LovaDbContext _context;

        public PartyModel(LovaDbContext context)
        {
            _context = context;
        }

        public DateTime DateToday { get; set; } = DateTime.Today;


        public async Task<JsonResult> OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            var DateToday = DateTime.Now;



            var Reservations = await _context.RentalReservations
                .Include(a => a.RentalInventory)
                .Where(a => a.PickupDate >= DateToday.AddDays(-7) && a.RentalInventory.GroupItems == "Party")
                .Select(m => new RentalSchedularViewModel
                {
                    RentalId = m.Id,
                    Title = m.RentalInventory.Name,
                    RentalInventoryId = m.RentalInventory.Id,
                    Start = m.PickupDate,
                    End = m.ReturnDate,
                    AspNetUserId = m.AspNetUserId,
                    BackgroundColor = m.RentalInventory.BackgroundColor
                })
                .ToListAsync();



            return new JsonResult(Reservations.ToDataSourceResult(request));
        }


        public async Task<JsonResult> OnPostCreate([DataSourceRequest] DataSourceRequest request, RentalSchedularViewModel rental)
        {
            if (ModelState.IsValid)
            {
                // var device = await _context.RentalInventories.Where(a => a.Name == rental.Title).FirstOrDefault();

                var ri = await _context.RentalInventories.Where(a => a.Id == rental.RentalInventoryId).FirstOrDefaultAsync();


                var insertData = new RentalReservation
                {
                    PickupDate = rental.Start,
                    ReturnDate = rental.End,
                    RentalInventoryId = rental.RentalInventoryId,
                    Description = rental.Description,
                    AspNetUserId = User.Identity.Name

                };

                await _context.RentalReservations.AddAsync(insertData);
                await _context.SaveChangesAsync();

                rental.RentalId = insertData.Id;
                rental.Title = ri.Name;
            }



            return new JsonResult(new[] { rental }.ToDataSourceResult(request, ModelState));
        }

    }
}