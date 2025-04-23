using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScooterRentalApp.Data;
using System;

namespace ScooterRentalApp.Controllers
{
    public class RentalController : Controller
    {
        private readonly ApplicationDbContext db;

        public RentalController(ApplicationDbContext applicationDbContext)
        {
            db = applicationDbContext;
        }

        // GET: Rental
        public ActionResult Index()
        {
            var rentals = db.Rentals
                .Include(r => r.Client)
                .Include(r => r.Scooter)
                .ToList();
            return View(rentals);
        }

        // GET: Rental/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Email");
            ViewBag.ScooterId = new SelectList(db.Scooters.Where(s => s.Status == "available"), "Id", "Model");
            return View();
        }

        // POST: Rental/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Rental rental)
        {
            if (ModelState.IsValid)
            {
                rental.RentalDate = DateTime.Now;
                rental.Cost = 0; 
                db.Rentals.Add(rental);


                var scooter = db.Scooters.Find(rental.Id);
                if (scooter != null)
                {
                    scooter.Status = "rented";
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "Email", rental.Id);
            ViewBag.ScooterId = new SelectList(db.Scooters, "ScooterId", "Model", rental.Id);
            return View(rental);
        }
    }
}
