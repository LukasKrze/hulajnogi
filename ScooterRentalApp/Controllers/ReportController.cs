using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScooterRentalApp.Data;
using ScooterRentalApp.Models;
using System;
using System.Runtime.CompilerServices;

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
        public ActionResult ProfitableCustomersTopTen(TopTenViewModel topTenViewModel)
        {
            SetReports();
            DateTime from, to;
            topTenViewModel = ProcessFilter(topTenViewModel, out from, out to);


            ViewBag.TopTenViewModel = topTenViewModel;

            var result = db.Clients
                .Select(c => new ProfitableCustomerViewModel
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    TotalIncome = c.Rentals
                                    .Where(r => r.RentalDate.Date >= from && r.RentalDate.Date <= to)
                                    .Sum(r => r.Payments.Sum(p => p.Amount))
                })
                .Where(c => c.TotalIncome > 0)
                .OrderByDescending(c => c.TotalIncome).Take(10);

            return View(result);
        }

        [HttpGet]
        public ActionResult DemandingCustomersTopTen(TopTenViewModel topTenViewModel)
        {
            SetReports();
            DateTime from, to;
            topTenViewModel = ProcessFilter(topTenViewModel, out from, out to);

            ViewBag.TopTenViewModel = topTenViewModel;

            var result = db.Clients
                .Select(c => new DemandingCustomerViewModel
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    NumberOfMessages = c.SupportMessages
                                            .Where(r => r.Created.Date >= from && r.Created.Date <= to)
                                            .Where(m => m.FromClient).Count()
                })
                .Where(c => c.NumberOfMessages > 0)
                .OrderByDescending(c => c.NumberOfMessages).Take(10);

            return View(result);
        }

        [HttpGet]
        public ActionResult RentedScootersTopTen(TopTenViewModel topTenViewModel)
        {
            SetReports();
            DateTime from, to;
            topTenViewModel = ProcessFilter(topTenViewModel, out from, out to);

            ViewBag.TopTenViewModel = topTenViewModel;

            var result = db.Scooters
                .Select(s => new RentedScooterViewModel
                {
                    Model = s.Model,
                    SerialNumber = s.SerialNumber,
                    NumberOfRentals = s.Rentals
                                           .Where(r => r.RentalDate.Date >= from && r.RentalDate.Date <= to)
                                           .Where(r => r.ReturnDate.HasValue || r.Scooter.CurrentRentalId.HasValue).Count()
                })
                .Where(c => c.NumberOfRentals > 0)
                .OrderByDescending(c => c.NumberOfRentals).Take(10);

            return View(result);
        }

        private TopTenViewModel ProcessFilter(TopTenViewModel topTenViewModel, out DateTime from, out DateTime to)
        {
            if (topTenViewModel.Mode == 0)
            {
                topTenViewModel.Mode = ReportMode.CustomRange;
            }

            if (topTenViewModel.Year == null || topTenViewModel.Year < 2020 || topTenViewModel.Year > 2120)
            {
                topTenViewModel.Year = DateTime.Today.Year;
            }

            switch (topTenViewModel.Mode)
            {
                case ReportMode.WholeYear:
                    topTenViewModel.From = new DateTime(topTenViewModel.Year.Value, 1, 1);
                    topTenViewModel.To = new DateTime(topTenViewModel.Year.Value, 12, 31);
                    break;
                case ReportMode.Q1:
                    topTenViewModel.From = new DateTime(topTenViewModel.Year.Value, 1, 1);
                    topTenViewModel.To = new DateTime(topTenViewModel.Year.Value, 3, 31);
                    break;
                case ReportMode.Q2:
                    topTenViewModel.From = new DateTime(topTenViewModel.Year.Value, 4, 1);
                    topTenViewModel.To = new DateTime(topTenViewModel.Year.Value, 6, 30);
                    break;
                case ReportMode.Q3:
                    topTenViewModel.From = new DateTime(topTenViewModel.Year.Value, 7, 1);
                    topTenViewModel.To = new DateTime(topTenViewModel.Year.Value, 9, 30);
                    break;
                case ReportMode.Q4:
                    topTenViewModel.From = new DateTime(topTenViewModel.Year.Value, 10, 1);
                    topTenViewModel.To = new DateTime(topTenViewModel.Year.Value, 12, 31);
                    break;
            }

            from = topTenViewModel.From ?? DateTime.MinValue;
            to = topTenViewModel.To ?? DateTime.MaxValue;

            return topTenViewModel;
        }

        private void SetReports([CallerMemberName] string actionName = null)
        {
            ViewBag.CurentReport = actionName;
            ViewBag.AllReports = new Dictionary<string, string>{
                {nameof(ProfitableCustomersTopTen),"10 najbardziej dochodowych klientów" },
                {nameof(DemandingCustomersTopTen),"10 najbardziej roszczeniowych klientów" },
                {nameof(RentedScootersTopTen),"10 najwięcej razy wypożyczanych hulajnóg" }
            };

        }
    }
}
