using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScooterRentalApp.Data;
using System;

namespace ScooterRentalApp.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext db;

        public ReservationController(ApplicationDbContext applicationDbContext)
        {
            db = applicationDbContext;
        }

        // GET: Rental
        public ActionResult Index()
        {
            var rentals = db.Rentals
                .Include(r => r.Client)
                .Include(r => r.Scooter)                
                .Where(r=> r.Client.UserName.ToLower() == Request.HttpContext.User.Identity.Name.ToLower())
                .ToList();
            return View(rentals);
        }

        // GET: Rental/Create
        public ActionResult Create(int scooterId)
        {
            var scooter = db.Scooters.FirstOrDefault(s => s.Id == scooterId);
            if (scooter == null)
            {
                return RedirectToAction("Index");
            }

            return View(new Rental { Scooter = scooter, RentalDate= DateTime.Now.AddMinutes(5), PlannedReturnDate = DateTime.Now.AddHours(4) });
        }

        // POST: Rental/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int scooterId, Rental rental)
        {
            var scooter = db.Scooters.Include(s=>s.Rentals).Include(s => s.Pricings).FirstOrDefault(s => s.Id == scooterId);
            rental.Scooter = scooter;
            rental.Pricing = scooter.Pricings.FirstOrDefault();
            if (rental.RentalDate < DateTime.Now)
            {
                ViewBag.Validation = "Nie można rezerwować z datą przeszłą";
                return View(rental);
            }

            if (scooter.Rentals.Any(r => r.RentalDate >= rental.PlannedReturnDate || (r.ReturnDate ?? r.PlannedReturnDate) <= rental.RentalDate))
            {
                ViewBag.Validation = "Istnieje już rezerwacja w podanym terminie";
                return View(rental);
            }

            rental.Client = db.Clients.FirstOrDefault(s => s.UserName.ToLower() == Request.HttpContext.User.Identity.Name.ToLower());
            db.Rentals.Add(rental);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Rent(int id)
        {
            var rental = db.Rentals.Include(r=>r.Scooter).FirstOrDefault(r => r.Id == id);
            if (rental != null && rental.Scooter!= null && rental.Scooter.CurrentRentalId == null)
            {
                rental.Scooter.CurrentRentalId = rental.Id;
                rental.RentalDate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public ActionResult Return(int id)
        {
            var rental = db.Rentals.Include(r => r.Scooter).FirstOrDefault(r => r.Id == id);
            if (rental != null && rental.Scooter != null && rental.Scooter.CurrentRentalId == id)
            {
                rental.Scooter.CurrentRentalId = null;
                rental.ReturnDate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
