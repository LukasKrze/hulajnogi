using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScooterRentalApp.Data;
using ScooterRentalApp.Models;
using System;

namespace ScooterRentalApp.Controllers
{
    [Authorize(Roles = "ADMINISTRATOR")]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext db;

        public ReportController(ApplicationDbContext applicationDbContext)
        {
            db = applicationDbContext;
        }

        [HttpGet]
        public ActionResult ProfitableCustomersTopTen()
        {
            var result = db.Clients
                .Select(c => new ProfitableCustomerViewModel
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    TotalIncome = c.Rentals.Sum(r => r.Payments.Sum(p => p.Amount))
                })
                .Where(c => c.TotalIncome > 0)
                .OrderByDescending(c => c.TotalIncome).Take(10);

            return View(result);
        }

        [HttpGet]
        public ActionResult DemandingCustomersTopTen()
        {
            var result = db.Clients
                .Select(c => new DemandingCustomerViewModel
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    NumberOfMessages = c.SupportMessages.Where(m=> m.FromClient).Count()
                })
                .Where(c => c.NumberOfMessages > 0)
                .OrderByDescending(c => c.NumberOfMessages).Take(10);

            return View(result);
        }

        [HttpGet]
        public ActionResult RentedScootersTopTen()
        {
            var result = db.Scooters
                .Select(s => new RentedScooterViewModel
                {
                    Model = s.Model,
                    SerialNumber = s.SerialNumber,                    
                    NumberOfRentals = s.Rentals.Where(r => r.ReturnDate.HasValue || r.Scooter.CurrentRentalId.HasValue).Count()
                })
                .Where(c => c.NumberOfRentals > 0)
                .OrderByDescending(c => c.NumberOfRentals).Take(10);

            return View(result);
        }

    }
}
