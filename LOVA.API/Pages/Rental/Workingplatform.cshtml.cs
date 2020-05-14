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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages.Rental
{
    [Authorize(Roles = "User, Admin, Styrelse")]
    public class WorkingplatformModel : PageModel
    {
        private readonly LovaDbContext _context;

        public WorkingplatformModel(LovaDbContext context)
        {
            _context = context;
        }

        public DateTime DateToday { get; set; }  = DateTime.Today;


        public async Task<JsonResult> OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            var DateToday = DateTime.Now;

            var Reservations = await _context.RentalReservations
                .Include(a => a.RentalInventory)
                .Where(a => a.RentalInventoryId == 1 && a.PickupDate >= DateToday)
                .Select( m => new RentalSchedularViewModel
                {
                    RentalId = m.Id,
                    Title = m.RentalInventory.Name,
                    Start = m.PickupDate,
                    End = m.ReturnDate
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
                    RentalInventoryId = 1,  // Id för Byggställning
                    Description = rental.Description

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

    }
}