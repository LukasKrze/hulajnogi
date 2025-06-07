using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScooterRentalApp.Data;
using ScooterRentalApp.Models;
using System;

namespace ScooterRentalApp.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext db;

        public PaymentController(ApplicationDbContext applicationDbContext)
        {
            db = applicationDbContext;
        }

        public ActionResult Index()
        {
            var rentals = db.Payments
                .Include(r => r.Rental).ThenInclude(r => r.Client)
                .Include(r => r.Rental).ThenInclude(r => r.Scooter)
                .AsQueryable();

            if (!User.IsInRole(SystemRoles.Administrator))
            {
                rentals = rentals.Where(r => r.Rental.Client.UserName.ToLower() == Request.HttpContext.User.Identity.Name.ToLower());
            }

            return View(rentals);
        }

        [HttpGet]
        public ActionResult ConfirmPaymentAtRental(int rentalId)
        {
            var rental = db.Rentals.Include(r => r.Scooter).ThenInclude(s => s.Pricings).Include(r => r.Pricing).Include(r => r.Payments).FirstOrDefault(r => r.Id == rentalId);
            var payment = new PaymentViewModel
            {
                From = DateTime.Now,
                To = rental.ReturnDate,
                PlannedTo = rental.PlannedReturnDate,
                ScooterPicture = rental.Scooter.Picture,
                Model = rental.Scooter.Model,
                Price = rental.Scooter.Pricings.Last().PricePerUnit,
                RecentCost = rental.Payments.Sum(p => p.Amount)
            };

            payment.CalculateCost();

            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmPaymentAtRental(int rentalId, PaymentViewModel paymentViewModel)
        {

            if (User.IsInRole(SystemRoles.Administrator))
            {
                return RedirectToAction("Index");
            }


            var rental = db.Rentals.Include(r => r.Scooter).Include(r => r.Payments).FirstOrDefault(r => r.Id == rentalId);
            if (rental != null && rental.Scooter != null && rental.Scooter.CurrentRentalId == null)
            {
                MapPayment(rental, paymentViewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public ActionResult ConfirmPaymentAtReturn(int rentalId)
        {
            var rental = db.Rentals.Include(r => r.Scooter).Include(r => r.Pricing).Include(r => r.Payments).FirstOrDefault(r => r.Id == rentalId);
            var payment = new PaymentViewModel
            {
                From = rental.RentalDate,
                To = DateTime.Now,
                PlannedTo = rental.PlannedReturnDate,
                ScooterPicture = rental.Scooter.Picture,
                Model = rental.Scooter.Model,
                Price = rental.Scooter.Pricings.Last().PricePerUnit,
                RecentCost = rental.Payments.Sum(p => p.Amount)
            };

            payment.CalculateCost();

            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmPaymentAtReturn(int rentalId, PaymentViewModel paymentViewModel)
        {

            if (User.IsInRole(SystemRoles.Administrator))
            {
                return RedirectToAction("Index");
            }


            var rental = db.Rentals.Include(r => r.Scooter).Include(r => r.Payments).FirstOrDefault(r => r.Id == rentalId);
            if (rental != null && rental.Scooter != null && rental.Scooter.CurrentRentalId == rentalId)
            {
                MapPayment(rental, paymentViewModel, true);
                db.SaveChanges();
                return paymentViewModel.Complaint ? RedirectToAction("Complaint", "SupportMessage", new { rentalId }) : RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        private void MapPayment(Rental rental, PaymentViewModel paymentViewModel, bool isReturn = false)
        {
            rental.Scooter.CurrentRentalId = !isReturn ? paymentViewModel.RentalId : null;
            rental.RentalDate = paymentViewModel.From;
            rental.PlannedReturnDate = paymentViewModel.PlannedTo;
            rental.ReturnDate = paymentViewModel.To;
            var payment = new Payment { Amount = paymentViewModel.Cost, Date = DateTime.Now, Rental = rental };
            rental.Payments.Add(payment);
            db.Payments.Add(payment);

        }
    }
}
