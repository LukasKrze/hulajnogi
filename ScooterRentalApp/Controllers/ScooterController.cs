using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScooterRentalApp.Data;
using System;

namespace ScooterRentalApp.Controllers
{
    public class ScooterController : Controller
    {
        private readonly ApplicationDbContext db;

        public ScooterController(ApplicationDbContext applicationDbContext)
        {
            db = applicationDbContext;
        }

        // GET: Scooter
        public ActionResult Index()
        {
            var scooters = db.Scooters.ToList();
               
            return View(scooters);
        }

    }
}
