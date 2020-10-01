using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages.Rental
{
    public class TrailerModel : PageModel
    {

        private readonly LovaDbContext _context;

        public TrailerModel(LovaDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }
        public DateTime DateToday { get; set; } = DateTime.Today;


        public async Task<JsonResult> OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            var DateToday = DateTime.Now;



            var Reservations = await _context.RentalReservations
                .Include(a => a.RentalInventory)
                .Where(a => a.PickupDate >= DateToday.AddDays(-7) && a.RentalInventory.GroupItems == "Slap")
                .Select(m => new RentalSchedularViewModel
                {
                    RentalId = m.Id,
                    Title = m.RentalInventory.Name,
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

                var insertData = new RentalReservation
                {
                    PickupDate = rental.Start,
                    ReturnDate = rental.End,
                    RentalInventoryId = 2,  // Id för Byggställning
                    Description = rental.Description,
                    AspNetUserId = User.Identity.Name,
                    NumberOf = 1


                };

                await _context.RentalReservations.AddAsync(insertData);
                await _context.SaveChangesAsync();

                rental.RentalId = insertData.Id;
            }



            return new JsonResult(new[] { rental }.ToDataSourceResult(request, ModelState));
        }

        public async Task<JsonResult> OnPostUpdate([DataSourceRequest] DataSourceRequest request, RentalSchedularViewModel rental)
        {
            if (ModelState.IsValid)
            {
                var updateData = await _context.RentalReservations
                    .Where(a => a.Id == rental.RentalId).FirstOrDefaultAsync();

                if (updateData == null)
                {
                    ModelState.AddModelError("Finns ej", "Finns inte i databasen.");
                    return new JsonResult(new[] { rental }.ToDataSourceResult(request, ModelState));
                }

                updateData.Description = rental.Description;
                updateData.PickupDate = rental.Start;
                updateData.ReturnDate = rental.End;

                await _context.RentalReservations.AddAsync(updateData);
                _context.Entry(updateData).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }


            return new JsonResult(new[] { rental }.ToDataSourceResult(request, ModelState));
        }


        //public async Task<JsonResult> OnGetNumber([DataSourceRequest] DataSourceRequest request, int rentalId, DateTime startDate, DateTime endDate)
        public async Task<JsonResult> OnGetNumber(int rentalId, DateTime startDate, DateTime endDate)
        {


            var booked = await _context.RentalReservations
                .Where(a => a.PickupDate >= startDate && a.ReturnDate <= endDate && a.RentalInventoryId == rentalId)
                .ToArrayAsync();

            var sum = (from x in booked select x.NumberOf).Sum();

            var numberOfItems = await _context.RentalInventories.Where(a => a.Id == rentalId).FirstOrDefaultAsync();

            int free = numberOfItems.NumberOf - sum;

            List<SelectListItem> freeToBook = new List<SelectListItem>();

            if (free > 0)
            {
                for (int i = 1; i < free + 1; i++)
                {
                    freeToBook.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                }

            }
            else
            {
                freeToBook.Add(new SelectListItem { Text = "Ingen ledig för vald period.", Value = "999" });
            }

            //return new JsonResult(new[] { freeToBook }.ToDataSourceResult(request));
            return new JsonResult(freeToBook);

        }



    }
}