using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ScooterRentalApp.Data
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<Client> userManager)
        {
            if (context.Roles.FirstOrDefault(r => r.Name == SystemRoles.Administrator) == null)
            {
                context.Add(new IdentityRole { Name = SystemRoles.Administrator, NormalizedName = SystemRoles.Administrator });
                context.SaveChanges();
            }

            var adminAccount = "Admin25@scooterrentalapp.pl";

            var user = userManager.FindByNameAsync(adminAccount).GetAwaiter().GetResult();

            if (user == null)
            {
                user = new Client
                {
                    UserName = adminAccount,
                    Email = adminAccount,
                    EmailConfirmed = true,
                    FirstName = "Konto",
                    LastName = "Administracyjne",
                    PhoneNumber = string.Empty
                };

                var result = userManager.CreateAsync(user, adminAccount).GetAwaiter().GetResult();
            }
            if (!userManager.IsInRoleAsync(user, SystemRoles.Administrator).GetAwaiter().GetResult())
            {
                userManager.AddToRoleAsync(user, SystemRoles.Administrator).GetAwaiter().GetResult();
            }

            if (!context.Scooters.OfType<ElectricScooter>().Any(s => s.Model == "E125"))
            {
                var pricing = new Pricing
                {
                    From = DateTime.MinValue,
                    To = DateTime.MaxValue,
                    PricePerUnit = 2
                };

                var scooter = new ElectricScooter
                {
                    Model = "E125",
                    BatteryCapacity = 120,
                    HasKickstand = false,
                    MaxSpeed = 30,
                    Range = 40,
                    SerialNumber = "XYZ1234",
                    YearOfProduction = 2022,
                    Pricings = new List<Pricing> { pricing }
                };
                
                pricing.Scooter = scooter;

                context.Pricings.Add(pricing);
                context.Scooters.Add(scooter);
                context.SaveChanges();
            }

            if (!context.Scooters.OfType<ManualScooter>().Any(s => s.Model == "M55"))
            {
                var pricing = new Pricing
                {
                    From = DateTime.MinValue,
                    To = DateTime.MaxValue,
                    PricePerUnit = 2
                };
                
                var manualScooter = new ManualScooter
                {
                    Model = "M55",
                    WheelSize = 15,
                    HasKickstand = false,
                    MaxSpeed = 30,
                    Range = 40,
                    SerialNumber = "ABC32",
                    YearOfProduction = 2022,
                    Pricings = new List<Pricing> { pricing }
                };
                
                pricing.Scooter = manualScooter;

                context.Pricings.Add(pricing);
                context.Scooters.Add(manualScooter);
                context.SaveChanges();
            }

        }
    }
}
