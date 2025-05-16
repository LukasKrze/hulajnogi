using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScooterRentalApp.Data;
using ScooterRentalApp.Models;
using ScooterRentalApp.Models.Validators;

namespace ScooterRentalApp.Controllers
{
    public class ScooterController : Controller
    {
        private readonly IValidator<ScooterViewModel> _validator;
        private readonly ApplicationDbContext _context;

        public ScooterController(IValidator<ScooterViewModel> validator, ApplicationDbContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Scooters.Select(s => ScooterViewModel.MapToViewModel(s)).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scooter = await _context.Scooters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scooter == null)
            {
                return NotFound();
            }

            return View(ScooterViewModel.MapToViewModel(scooter));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BatteryCapacity,Id,Model,SerialNumber,MaxSpeed,Range,Type,HasKickstand,YearOfProduction,InitialPrice,WheelSize")] ScooterViewModel scooter)
        {

            ValidationResult result = await _validator.ValidateAsync(scooter);

            if (ModelState.IsValid && result.IsValid)
            {
                var model = scooter.MapToModel();
                model.Pricings = new List<Pricing>
                { 
                    new Pricing { From = DateTime.Now, PricePerUnit = scooter.InitialPrice.Value } 
                };
                
                _context.Add(model);
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            result.AddToModelState(ModelState);

            return View(scooter);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scooter = await _context.Scooters.FindAsync(id);
            if (scooter == null)
            {
                return NotFound();
            }
            return View(ScooterViewModel.MapToViewModel(scooter));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BatteryCapacity,Id,Model,SerialNumber,MaxSpeed,Range,Type,HasKickstand,YearOfProduction,WheelSize")] ScooterViewModel scooter)
        {
            if (id != scooter.Id)
            {
                return NotFound();
            }

            ValidationResult result = await _validator.ValidateAsync(scooter);


            if (ModelState.IsValid && result.IsValid)
            {
                try
                {
                    _context.Update(scooter.MapToModel());
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScooterExists(scooter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            result.AddToModelState(ModelState);

            return View(scooter);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scooter = await _context.Scooters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scooter == null)
            {
                return NotFound();
            }

            return View(ScooterViewModel.MapToViewModel(scooter));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scooter = await _context.Scooters.FindAsync(id);
            if (scooter != null)
            {
                _context.Scooters.Remove(scooter);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScooterExists(int id)
        {
            return _context.Scooters.Any(e => e.Id == id);
        }
    }
}
